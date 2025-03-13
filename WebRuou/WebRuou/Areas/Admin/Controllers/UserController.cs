using System;
using System.Linq;
using System.Web.Mvc;
using WebRuou.Models;
using PagedList;

namespace WebRuou.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        DBRuouEntities db = new DBRuouEntities();

        // Hiển thị danh sách người dùng (có phân trang)
        public ActionResult Index(int? page)
        {
            int pageSize = 10; // Số người dùng trên mỗi trang
            int pageNumber = (page ?? 1);

            var users = db.Users
                          .OrderByDescending(u => u.CreatedAt)
                          .ToPagedList(pageNumber, pageSize);

            return View(users);
        }

        // Kích hoạt hoặc vô hiệu hóa tài khoản
        public ActionResult ToggleStatus(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Giả sử có trường IsActive để kiểm soát trạng thái tài khoản
            user.IsActive = !user.IsActive;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Xem danh sách sản phẩm yêu thích của người dùng
        public ActionResult Wishlist(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var wishlistItems = db.Wishlists.Where(w => w.UserID == id).ToList();

            return View(wishlistItems);
        }
    }
}
