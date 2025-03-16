using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebRuou.Models;
using System.Web.Helpers;

namespace WebRuou.Controllers
{
    public class LoginController : Controller
    {
        private DBRuouEntities db = new DBRuouEntities();

        // Trang đăng nhập
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !Crypto.VerifyHashedPassword(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
                return View("Index");
            }

            // Xác thực đăng nhập
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }, DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties(), identity);

            // Điều hướng theo vai trò
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        // Xử lý đăng nhập bằng Google
        public ActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", "Login", null, Request.Url.Scheme) };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        // Xử lý đăng nhập bằng Facebook
        public ActionResult LoginFacebook()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", "Login") };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Facebook");
            return new HttpUnauthorizedResult();
        }

        // Xử lý callback sau khi đăng nhập thành công
        public ActionResult ExternalLoginCallback()
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties(), identity);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Login", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, provider);
            return new HttpUnauthorizedResult();
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
