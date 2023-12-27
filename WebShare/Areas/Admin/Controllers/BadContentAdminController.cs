using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShare.Models;

namespace WebShare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/BadContentAdmin")]
    public class BadContentAdminController : Controller
    {
        WebShareContext db = new();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var lstBadContent = db.BadContents.ToList();
            return View(lstBadContent);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(BadContent objBadContent)
        {
            try
            {
                db.BadContents.Add(objBadContent);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã tạo nội dung xấu mới thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Tạo nội dung xấu mới thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objBadContent = db.BadContents.Where(n => n.Id == Id).FirstOrDefault();
            if (objBadContent == null)
            {
                return NotFound();
            }
            return View(objBadContent);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BadContent objBadContent)
        {
            try
            {
                // Cập nhật trạng thái của đối tượng objBadContent và lưu vào cơ sở dữ liệu
                db.Entry(objBadContent).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã sửa nội dung xấu thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa nội dung xấu thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        // Xoá
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objBadContent = db.BadContents.FirstOrDefault(n => n.Id == id);
            if (objBadContent != null)
            {
                try
                {
                    db.BadContents.Remove(objBadContent);
                    db.SaveChanges();

                    // Thêm thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đã xóa nội dung xấu thành công.";
                }
                catch (Exception ex)
                {
                    // Kiểm tra xem lỗi có phải là lỗi ràng buộc FOREIGN KEY không
                    if (ex is DbUpdateException && ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                    {
                        // Thêm thông báo lỗi vào TempData
                        TempData["ErrorMessage"] = "Lỗi! nội dung xấu đã có sản phẩm liên kết và không thể xóa.";
                    }
                    else
                    {
                        // Nếu không phải là lỗi ràng buộc, xử lý lỗi theo ý bạn ở đây
                        TempData["ErrorMessage"] = "Lỗi không xác định xảy ra khi xóa nội dung xấu.";
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                // Thêm thông báo lỗi vào TempData nếu cần
                TempData["ErrorMessage"] = "Lỗi! Không tìm thấy nội dung xấu để xóa.";

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
                var objBadContent = db.BadContents.FirstOrDefault(n => n.Id == id);
                if (objBadContent != null)
                {
                    db.BadContents.Remove(objBadContent);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
