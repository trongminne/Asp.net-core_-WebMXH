using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShare.Models;

namespace WebShare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/SubjectAdmin")]
    public class SubjectAdminController : Controller
    {
        WebShareContext db = new();

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var lstSubject = db.Subjects.Include(s => s.IdCategoryNavigation).ToList();
            return View(lstSubject);
        }
        [Route("Create")]
        public IActionResult Create()
        {
            // Lấy danh sách danh mục và đưa vào SelectList
            var categories = db.Categories.ToList();
            var categoryList = new SelectList(categories, "Id", "Name").ToList();

            // Đưa SelectList vào ViewData để có thể truy cập từ trong view
            ViewData["CategoryList"] = categoryList;
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(Subject objSubject)
        {
            try
            {
                db.Subjects.Add(objSubject);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã tạo môn học mới thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Tạo môn học mới thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            // Lấy danh sách danh mục và đưa vào SelectList
            var categories = db.Categories.ToList();
            var categoryList = new SelectList(categories, "Id", "Name").ToList();

            // Đưa SelectList vào ViewData để có thể truy cập từ trong view
            ViewData["CategoryList"] = categoryList;

            var objSubject = db.Subjects.Where(n => n.Id == Id).FirstOrDefault();
            if (objSubject == null)
            {
                return NotFound();
            }
            return View(objSubject);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subject objSubject)
        {
            try
            {
                // Cập nhật trạng thái của đối tượng objSubject và lưu vào cơ sở dữ liệu
                db.Entry(objSubject).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đã sửa môn học thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể ghi log hoặc thông báo lỗi chi tiết
                TempData["ErrorMessage"] = "Sửa môn học thất bại. Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


        // Xoá
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var objSubject = db.Subjects.FirstOrDefault(n => n.Id == id);
            if (objSubject != null)
            {
                try
                {
                    db.Subjects.Remove(objSubject);
                    db.SaveChanges();

                    // Thêm thông báo thành công vào TempData
                    TempData["SuccessMessage"] = "Đã xóa môn học thành công.";
                }
                catch (Exception ex)
                {
                    // Kiểm tra xem lỗi có phải là lỗi ràng buộc FOREIGN KEY không
                    if (ex is DbUpdateException && ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                    {
                        // Thêm thông báo lỗi vào TempData
                        TempData["ErrorMessage"] = "Lỗi! môn học đã có sản phẩm liên kết và không thể xóa.";
                    }
                    else
                    {
                        // Nếu không phải là lỗi ràng buộc, xử lý lỗi theo ý bạn ở đây
                        TempData["ErrorMessage"] = "Lỗi không xác định xảy ra khi xóa môn học.";
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                // Thêm thông báo lỗi vào TempData nếu cần
                TempData["ErrorMessage"] = "Lỗi! Không tìm thấy môn học để xóa.";

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
                var objSubject = db.Subjects.FirstOrDefault(n => n.Id == id);
                if (objSubject != null)
                {
                    db.Subjects.Remove(objSubject);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
