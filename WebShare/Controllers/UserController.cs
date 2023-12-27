using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Controllers
{
	public class UserController : Controller
	{
		private readonly ImageHelper _imageHelper;

		public UserController(ImageHelper imageHelper)
		{
			_imageHelper = imageHelper;
		}

		WebShareContext db = new();
		[HttpGet]
		public IActionResult Profile(int idUser)
		{
			var user = db.Users.Find(idUser);
			return View(user);
		}
        [HttpPost]
        public IActionResult Edit(int id, string Password, IFormFile Files)
        {
            var User = db.Users.Find(id);
            if(User.Password != Password)
            {
                TempData["ErrorMessage"] = "Sai mật khẩu";
                return RedirectToAction("Index", "Group");
            }
            else
            {
                try
                {
                    if (Files != null)
                    {
                        User.Avatar = _imageHelper.SaveImage(Files);
                    }
                    else
                    {
                        db.Entry(User).Property(x => x.Avatar).IsModified = false;
                    }
                    db.Entry(User).State = EntityState.Modified;

                    db.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Lưu tài khoản thành công.";
                    return RedirectToAction("Logout", "Home");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                    TempData["ErrorMessage"] = "Lưu tài khoản thất bại. Lỗi: " + ex.Message;
                    return RedirectToAction("Index", "Group");

                }
            }
           
        }
    }
}
