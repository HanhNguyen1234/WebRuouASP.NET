using System;
using System.Linq;
using System.Web.Mvc;
using WebRuou.Models;
using System.Web.Helpers;

namespace WebRuou.Controllers
{
    public class RegisterController : Controller
    {
        private DBRuouEntities db = new DBRuouEntities();

        // Hiển thị form đăng ký
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Email đã tồn tại!";
                    return View("Index", model);
                }

                if (model.Password != ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "Mật khẩu nhập lại không khớp!";
                    return View("Index", model);
                }

                // Mã hóa mật khẩu trước khi lưu
                model.PasswordHash = Crypto.HashPassword(model.Password);
                model.CreatedAt = DateTime.Now;
                model.IsActive = true;

                // Xác định vai trò dựa vào email
                if (model.Email.EndsWith("@admin.com"))
                {
                    model.Role = "Admin";
                }
                else
                {
                    model.Role = "User";
                }

                db.Users.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index", "Login");
            }

            return View("Index", model);
        }
    }
}
