using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(WebRuou.Startup))]
namespace WebRuou
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        private void ConfigureAuth(IAppBuilder app)
        {
            // 1️⃣ Cấu hình Cookie Authentication (Phải có trước các OAuth Provider)
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            });

           /* // 2️⃣ Cấu hình Google Authentication
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"),
                CallbackPath = new PathString("/signin-google"),
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie // ⭐ Cần có
            });*/

            // 3️⃣ Cấu hình Facebook Authentication
            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = "1329580451589504",
                AppSecret = "1bcee80af95578ccc5758a2f9ed0d4ac",
                CallbackPath = new PathString("/signin-facebook"),
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie // ⭐ Cần có
            });

            // 4️⃣ Cấu hình External Cookie Authentication (Fix lỗi chính)
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}