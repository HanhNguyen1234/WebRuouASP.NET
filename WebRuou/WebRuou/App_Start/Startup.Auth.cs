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
            LoginPath = new PathString("/Login")
        });

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
            AppId = "1169311708193078",
            AppSecret = "7177e80d40ce564a512157dc4c88b6e9",
            CallbackPath = new PathString("/signin-facebook")
        });
    }
}
