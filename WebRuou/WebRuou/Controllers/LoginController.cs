using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebRuou.Models;

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
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return RedirectToAction("Index", "Login");
            }

            using (var db = new DBRuouEntities())
            {
                // Kiểm tra xem email có trong bảng AdminUsers không
                var adminUser = db.AdminUsers.SingleOrDefault(a => a.Username == loginInfo.Email);

                if (adminUser != null)
                {
                    // Nếu tài khoản có trong AdminUsers -> Đăng nhập với quyền Admin
                    var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.Name, adminUser.Username)); // Dùng Username thay vì FullName
                    identity.AddClaim(new Claim(ClaimTypes.Email, adminUser.Username));
                    identity.AddClaim(new Claim(ClaimTypes.Role, adminUser.Role ?? "User")); // Mặc định User nếu Role null

                    HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties(), identity);
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                // Nếu không phải admin, kiểm tra trong bảng Users
                var user = db.Users.SingleOrDefault(u => u.Email == loginInfo.Email);

                if (user == null)
                {
                    // Nếu user chưa tồn tại, thêm vào bảng Users
                    var newUser = new User
                    {
                        FullName = loginInfo.DefaultUserName ?? loginInfo.Email,
                        Email = loginInfo.Email,
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    // Thêm vào AdminUsers với role mặc định là "User"
                    var newAdminUser = new AdminUser
                    {
                        Username = loginInfo.Email,
                        PasswordHash = "", // Nếu cần mật khẩu cho đăng nhập thông thường thì cập nhật sau
                        Role = "User"
                    };
                    db.AdminUsers.Add(newAdminUser);
                    db.SaveChanges();
                }

                // Đăng nhập với quyền User
                var userIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, user?.FullName ?? loginInfo.Email));
                userIdentity.AddClaim(new Claim(ClaimTypes.Email, loginInfo.Email));
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties(), userIdentity);

                return RedirectToAction("Index", "Index");
            }
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