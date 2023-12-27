using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShare.Models;

namespace WebShare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/CategoryAdmin")]
    public class CategoryAdminController : Controller
    {
        WebShareContext db = new ();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var lstCategory = db.Categories.ToList();
            return View(lstCategory);
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(Category objCategory)
        {
            try
            {
                db.Categories.Add(objCategory);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã tạo lĩnh vực mới thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Tạo lĩnh vực mới thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objCategory = db.Categories.Where(n => n.Id == Id).FirstOrDefault();
            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category objCategory)
        {
            try
            {
                // Cập nhật trạng thái của đối tượng objCategory và lưu vào cơ sở dữ liệu
                db.Entry(objCategory).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã sửa lĩnh vực thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa lĩnh vực thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        // Xoá
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objCategory = db.Categories.FirstOrDefault(n => n.Id == id);
            if (objCategory != null)
            {
                try
                {
                    db.Categories.Remove(objCategory);
                    db.SaveChanges();

                    // Thêm thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đã xóa lĩnh vực thành công.";
                }
                catch (Exception ex)
                {
                    // Kiểm tra xem lỗi có phải là lỗi ràng buộc FOREIGN KEY không
                    if (ex is DbUpdateException && ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                    {
                        // Thêm thông báo lỗi vào TempData
                        TempData["ErrorMessage"] = "Lỗi! lĩnh vực đã có sản phẩm liên kết và không thể xóa.";
                    }
                    else
                    {
                        // Nếu không phải là lỗi ràng buộc, xử lý lỗi theo ý bạn ở đây
                        TempData["ErrorMessage"] = "Lỗi không xác định xảy ra khi xóa lĩnh vực.";
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                // Thêm thông báo lỗi vào TempData nếu cần
                TempData["ErrorMessage"] = "Lỗi! Không tìm thấy lĩnh vực để xóa.";

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
                var objCategory = db.Categories.FirstOrDefault(n => n.Id == id);
                if (objCategory != null)
                {
                    db.Categories.Remove(objCategory);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
