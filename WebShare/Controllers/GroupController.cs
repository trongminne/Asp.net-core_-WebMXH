using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Controllers
{

    public class GroupController : Controller
    {
        private readonly ImageHelper _imageHelper;

        public GroupController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        WebShareContext db = new();
        public IActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();


            // Lấy danh sách bài viết từ cơ sở dữ liệu
            var posts = db.Posts
                .Include(p => p.Comments)
                .Include(p => p.IdUserNavigation)
                .Include(p => p.Likes)
                    .ThenInclude(l => l.IdUserNavigation)
                .ToList();

            // Lấy danh sách 5 user đầu tiên và số lượng like từ mỗi user
            var topLikers = posts
                .SelectMany(p => p.Likes)
                .GroupBy(l => l.IdUserNavigation)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new LikedUser { User = g.Key, Count = g.Count() })
                .ToList();

            // Tạo ViewModel
            var viewModel = new CombinedViewModel
            {
                BlogViewModel = new BlogViewModel
                {
                    Posts = posts,
                    TopLikers = topLikers
                },
                Posts = posts
            };

            // Trả về view và chuyển dữ liệu qua tham số
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Post(Post objPost, List<IFormFile> Files)
        {
            // Kiểm tra nội dung xấu
            if (HasBadContent(objPost.Contents))
            {
                TempData["ErrorMessage"] = "Bài viết vi phạm nội dung";
                return RedirectToAction("Index");
            }

            if (Files != null && Files.Count > 0)
            {
                string fileNames = string.Join(",", Files.Select(file => _imageHelper.SaveImage(file)));
                objPost.Filename = fileNames;
            }
            objPost.DatePost = DateTime.Now;
            TempData["SuccessMessage"] = "Đăng bài thành công";
            db.Posts.Add(objPost);
            db.SaveChanges();
            // Lưu thông tin vào CSDL (hiện tại chỉ return View() cho mục đích minh họa)
            return RedirectToAction("Index");
        }

        private bool HasBadContent(string contents)
        {
            // Lấy danh sách nội dung xấu từ CSDL
            var badContents = db.BadContents.Select(bc => bc.Badcontent1).ToList();

            // Kiểm tra nếu contents chứa bất kỳ từ nào trong danh sách nội dung xấu
            return badContents.Any(bc => contents.IndexOf(bc, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [HttpGet]
        public JsonResult GetSubjectsByCategoryId(int categoryId)
        {
            var subjects = db.Subjects.Where(s => s.IdCategory == categoryId).ToList();
            return Json(subjects);
        }

        [HttpPost]
        public IActionResult Search(string name, int id)
        {
            ViewBag.Categories = db.Categories.ToList();


            // Lấy danh sách bài viết từ cơ sở dữ liệu
            var posts = db.Posts
                .Include(p => p.Comments)
                .Include(p => p.IdUserNavigation)
                .Include(p => p.Likes)
                    .ThenInclude(l => l.IdUserNavigation)
                .Where(p => p.IdSub == id || p.Contents.Contains(name))
                .ToList();

            // Lấy danh sách 5 user đầu tiên và số lượng like từ mỗi user
            var topLikers = posts
                .SelectMany(p => p.Likes)
                .GroupBy(l => l.IdUserNavigation)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new LikedUser { User = g.Key, Count = g.Count() })
                .ToList();

            // Tạo ViewModel
            var viewModel = new CombinedViewModel
            {
                BlogViewModel = new BlogViewModel
                {
                    Posts = posts,
                    TopLikers = topLikers
                },
                Posts = posts
            };

            // Trả về view và chuyển dữ liệu qua tham số
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Comment(Comment objComment, List<IFormFile> FilesComment)
        {
            // Kiểm tra nội dung xấu
            if (HasBadContent(objComment.Contents))
            {
                TempData["ErrorMessage"] = "Comment vi phạm nội dung";
                return RedirectToAction("Index");
            }

            if (FilesComment != null && FilesComment.Count > 0)
            {
                string fileNames = string.Join(",", FilesComment.Select(file => _imageHelper.SaveImage(file)));
                objComment.Img = fileNames;
            }
            objComment.DatePost = DateTime.Now;
            TempData["SuccessMessage"] = "Bình luận thành công";
            db.Comments.Add(objComment);
            db.SaveChanges();
            // Lưu thông tin vào CSDL (hiện tại chỉ return View() cho mục đích minh họa)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult LikePost(Like objLike, int postId, int userId)
        {
            // Thực hiện logic xử lý Like ở đây
            objLike.IdPost = postId;
            objLike.IdUser = userId;
            db.Likes.Add(objLike);
            db.SaveChanges();

            // Trả về dữ liệu mới (số lượng like sau khi thực hiện)
            var likeCount = db.Likes.Count(l => l.IdPost == postId);
            return Json(new { LikeCount = likeCount });
        }
        [HttpPost]
        public JsonResult LikeComment(LikeComment objLikeComment, int commentId, int userId)
        {
            // Thực hiện logic xử lý Like ở đây
            objLikeComment.IdComment = commentId;
            objLikeComment.IdUser = userId;
            db.LikeComments.Add(objLikeComment);
            db.SaveChanges();

            // Trả về dữ liệu mới (số lượng like sau khi thực hiện)
            var likeCount = db.LikeComments.Count(l => l.IdComment == commentId);
            return Json(new { LikeCount = likeCount });
        }

        [HttpGet]
        public IActionResult DetailPost(int postId)
        {
            var post = db.Posts.Find(postId);
            return View(post);
        }
    }
}
