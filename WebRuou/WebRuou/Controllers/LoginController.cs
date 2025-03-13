using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebRuou.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        // Trang đăng nhập
        public ActionResult Index()
        {
            return View();
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
                return RedirectToAction("Index","Index");
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