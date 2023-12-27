using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Controllers
{
    public class CommentController : Controller
    {
        private readonly ImageHelper _imageHelper;

        public CommentController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        WebShareContext db = new();
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Comment = db.Comments.Find(id);
            return View(Comment);
        }

        [HttpPost]
        public IActionResult Edit(int id, int IdPost, int idUser, string Contents, List<IFormFile> Files)
        {
            var Comment = db.Comments.Find(id);
            
            Comment.IdPost = IdPost;
            Comment.IdUser = idUser;
            try
            {
                if (Files != null && Files.Count > 0)
                {
                    string fileNames = string.Join(",", Files.Select(file => _imageHelper.SaveImage(file)));
                    Comment.Img = fileNames;
                }
                else
                {
                    db.Entry(Comment).Property(x => x.Img).IsModified = false;
                }
                db.Entry(Comment).State = EntityState.Modified;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Sửa bình luận thành công.";
                return RedirectToAction("Index", "Group");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa bình luận thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index", "Group");

            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Tìm comment cần xoá
            var objComment = db.Comments.FirstOrDefault(n => n.Id == id);

            // Kiểm tra xem comment có tồn tại hay không
            if (objComment != null)
            {
                // Xoá tất cả các LikeComment có IdComment tương ứng với Id của Comment
                var likeCommentsToRemove = db.LikeComments.Where(lc => lc.IdComment == objComment.Id);
                db.LikeComments.RemoveRange(likeCommentsToRemove);

                // Xoá Comment
                db.Comments.Remove(objComment);

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }

            // Chuyển hướng về trang Index của Group (hoặc trang khác nếu muốn)
            return RedirectToAction("Index", "Group");
        }



    }
}
