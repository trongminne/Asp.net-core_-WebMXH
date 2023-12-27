using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShare.Helpers;
using WebShare.Models;

namespace WebShare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/GroupAdmin")]
    [AdminAccess]
    public class GroupAdminController : Controller
    {
        private readonly ImageHelper _imageHelper;

        public GroupAdminController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
        }

        WebShareContext db = new();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var lstGroup = db.Groups.ToList();
            return View(lstGroup);
        }

        // Sửa
        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objGroup = db.Groups.Where(n => n.Id == Id).FirstOrDefault();
            if (objGroup == null)
            {
                return NotFound();
            }
            return View(objGroup);
        }
        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, IFormFile avatar, IFormFile avatarCover)
        {
            try
            {
                var objGroup = db.Groups.Where(n => n.Id == Id).FirstOrDefault();

                if (avatar != null)
                {
                    _imageHelper.DeleteImage(objGroup.Avatar);
                    objGroup.Avatar = _imageHelper.SaveImage(avatar);
                }
                else
                {
                    db.Entry(objGroup).Property(x => x.Avatar).IsModified = false;
                }
                if (avatarCover != null)
                {
                    _imageHelper.DeleteImage(objGroup.CoverImage);
                    objGroup.CoverImage = _imageHelper.SaveImage(avatarCover);
                }
                else
                {
                    db.Entry(objGroup).Property(x => x.CoverImage).IsModified = false;
                }

                db.Entry(objGroup).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã sửa Nhóm thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa nhóm thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
