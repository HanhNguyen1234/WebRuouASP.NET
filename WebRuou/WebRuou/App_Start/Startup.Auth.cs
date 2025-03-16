using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;

public partial class Startup
{
    public void ConfigureAuth(IAppBuilder app)
    {
        // Cấu hình Cookie Authentication
        app.UseCookieAuthentication(new CookieAuthenticationOptions
        {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Login"),
            ExpireTimeSpan = TimeSpan.FromMinutes(60),
            SlidingExpiration = true
        });
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


        /*// Đăng nhập bằng Google
        app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        {
            ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"),
            ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"),
            CallbackPath = new PathString("/signin-google")
        });*/

        // Đăng nhập bằng Facebook
        app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
        {
            AppId = "1329580451589504",
            AppSecret = "1bcee80af95578ccc5758a2f9ed0d4ac",
            CallbackPath = new PathString("/signin-facebook"),
             SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie
        });
    }
}
