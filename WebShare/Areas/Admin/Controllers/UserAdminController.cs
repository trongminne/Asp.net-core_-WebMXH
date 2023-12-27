using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/UserAdmin")]
    public class UserAdminController : Controller
    {
        private readonly ImageHelper _imageHelper;

        public UserAdminController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        WebShareContext db = new();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var listUser = db.Users.ToList();   
            return View(listUser);
        }


        // Xoá
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objUser = db.Users.FirstOrDefault(n => n.Id == id);
            if (objUser != null)
            {
                // Xoá ảnh cũ
                _imageHelper.DeleteImage(objUser.Avatar);
                try
                {
                    db.Users.Remove(objUser);
                    db.SaveChanges();

                    // Thêm thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đã xóa tài khoản thành công.";
                }
                catch (Exception ex)
                {
                    // Kiểm tra xem lỗi có phải là lỗi ràng buộc FOREIGN KEY không
                    if (ex is DbUpdateException && ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                    {
                        // Thêm thông báo lỗi vào TempData
                        TempData["ErrorMessage"] = "Lỗi! tài khoản đã có sản phẩm liên kết và không thể xóa.";
                    }
                    else
                    {
                        // Nếu không phải là lỗi ràng buộc, xử lý lỗi theo ý bạn ở đây
                        TempData["ErrorMessage"] = "Lỗi không xác định xảy ra khi xóa tài khoản.";
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                // Thêm thông báo lỗi vào TempData nếu cần
                TempData["ErrorMessage"] = "Lỗi! Không tìm thấy tài khoản để xóa.";

                return RedirectToAction("Index");
            }
        }

        // Xoá nhiều
        [Route("DeleteMultiple")]
        [HttpPost]
        public IActionResult DeleteMultiple(List<int> ids)
        {
            foreach (var id in ids)
            {
                var objUser = db.Users.FirstOrDefault(n => n.Id == id);
                if (objUser != null)
                {
                    // Xoá ảnh cũ
                    _imageHelper.DeleteImage(objUser.Avatar);
                    db.Users.Remove(objUser);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
