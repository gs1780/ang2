using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace AADConnect.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var idToken = await HttpContext.GetTokenAsync("id_token");
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                ViewData["IdToken"] = idToken ?? string.Empty;
                ViewData["AccessToken"] = accessToken ?? string.Empty;
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            var callbackUrl = Url.Action("AfterLogout", "Home", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public IActionResult AfterLogout()
        {
            return AfterLogout();
        }
    }
}
