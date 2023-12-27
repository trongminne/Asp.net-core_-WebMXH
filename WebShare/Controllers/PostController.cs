using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Controllers
{
    public class PostController : Controller
    {
        private readonly ImageHelper _imageHelper;

        public PostController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        WebShareContext db = new();
        [HttpGet]
        public IActionResult Edit(int postId)
        {
            // Lấy danh sách danh mục và đưa vào SelectList
            var subject = db.Subjects.ToList();
            var subjectList = new SelectList(subject, "Id", "Name").ToList();

            // Đưa SelectList vào ViewData để có thể truy cập từ trong view
            ViewData["subjectList"] = subjectList;
            var post = db.Posts.Find(postId);
            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(int id, int IdSub, string Contents, List<IFormFile> Files)
        {
            var Post = db.Posts.Find(id);
            // Kiểm tra nội dung xấu
            if (HasBadContent(Post.Contents))
            {
                TempData["ErrorMessage"] = "Bài viết vi phạm nội dung";
                return RedirectToAction("Index");
            }
            Post.IdSub = IdSub;
            Post.Contents = Contents;   

            try
            {
                if (Files != null && Files.Count > 0)
                {
                    string fileNames = string.Join(",", Files.Select(file => _imageHelper.SaveImage(file)));
                    Post.Filename = fileNames;
                }
                else
                {
                    db.Entry(Post).Property(x => x.Filename).IsModified = false;
                }
                db.Entry(Post).State = EntityState.Modified;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Sửa bài thành công.";
                return RedirectToAction("Index", "Group");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa bài thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index", "Group");

            }
        }
        private bool HasBadContent(string contents)
        {
            // Lấy danh sách nội dung xấu từ CSDL
            var badContents = db.BadContents.Select(bc => bc.Badcontent1).ToList();

            // Kiểm tra nếu contents chứa bất kỳ từ nào trong danh sách nội dung xấu
            return badContents.Any(bc => contents.IndexOf(bc, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        
        [HttpGet]
        public IActionResult Delete(int postId)
        {
            try
            {
                var post = db.Posts
                             .Include(p => p.Likes)
                                 .ThenInclude(l => l.IdUserNavigation)
                             .Include(p => p.Comments)
                                 .ThenInclude(c => c.IdUserNavigation)
                             .FirstOrDefault(p => p.Id == postId);

                if (post != null)
                {
                    // Xóa tất cả các like của comment
                    db.Likes.RemoveRange(post.Likes);
                    // Xóa tất cả các comment của post
                    db.Comments.RemoveRange(post.Comments);

                    // Xóa Post
                    db.Posts.Remove(post);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();
                }


                // Chuyển hướng đến trang Index hoặc nơi khác tùy vào yêu cầu của bạn
                return RedirectToAction("Index", "Group");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Xóa like thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index", "Group");
            }
        }
    }
}
