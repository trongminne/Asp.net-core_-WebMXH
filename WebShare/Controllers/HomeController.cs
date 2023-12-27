using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Security.Cryptography;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Controllers
{
	public class HomeController : Controller
	{
		private readonly ImageHelper _imageHelper;
		private readonly ILogger<HomeController> _logger;
		WebShareContext db = new();

		public HomeController(ILogger<HomeController> logger, ImageHelper imageHelper)
		{
			_logger = logger;
			_imageHelper = imageHelper;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(User loginUser)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra tài khoản và mật khẩu từ cơ sở dữ liệu
				var userFromDb = db.Users.SingleOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

				if (userFromDb != null)
				{
					// Lưu thông tin người dùng vào session
					HttpContext.Session.SetString("UserId", userFromDb.Id.ToString());

					HttpContext.Session.SetString("Username", userFromDb.Username);

					HttpContext.Session.SetString("Avatar", userFromDb.Avatar.ToString());

					HttpContext.Session.SetString("Role", userFromDb.Role.ToString());
					var session_Role = HttpContext.Session.GetString("Role");


					// Thông báo đăng nhập thành công
					TempData["SuccessMessage"] = "Đăng nhập thành công";
					if (session_Role != "")
					{
						return RedirectToAction("Index", "GroupAdmin", new { area = "Admin" });
					}
					else
					{
						return RedirectToAction("Index", "Group");
					}

				}
				else
				{
					// Thông báo đăng nhập không thành công
					TempData["ErrorMessage"] = "Sai tên đăng nhập hoặc mật khẩu";
				}
			}

			// Nếu dữ liệu không hợp lệ hoặc đăng nhập không thành công, quay lại trang đăng nhập với thông tin lỗi
			return RedirectToAction("");
		}
		public IActionResult Logout()
		{
			// Xóa thông tin đăng nhập khỏi session
			HttpContext.Session.Clear();
			// Xóa thông báo đăng nhập thành công
			TempData.Remove("SuccessMessage");
			// Nếu bạn muốn chuyển hướng đến trang khác sau khi đăng xuất, thêm URL mong muốn vào Redirect
			return RedirectToAction("Index", "Home");
		}

        [HttpPost]
        public IActionResult Register(User objUser, IFormFile avatar)
        {
            // Kiểm tra xem tên người dùng đã tồn tại hay chưa
            if (IsUsernameExists(objUser.Username))
            {
                // Nếu tên người dùng đã tồn tại, thì thông báo lỗi
                TempData["ErrorMessage"] = "Tài khoản này đã có người đăng ký.";
                return RedirectToAction("Index");
            }

            // Lưu hình ảnh và thêm người dùng mới vào cơ sở dữ liệu
            objUser.Avatar = _imageHelper.SaveImage(avatar);
            db.Users.Add(objUser);
            db.SaveChanges();

            // Thông báo thành công
            TempData["SuccessMessage"] = "Đăng kí thành công.";

            return RedirectToAction("Index");
        }

        // Hàm kiểm tra xem tên người dùng đã tồn tại hay chưa
        private bool IsUsernameExists(string username)
        {
            return db.Users.Any(u => u.Username == username);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}