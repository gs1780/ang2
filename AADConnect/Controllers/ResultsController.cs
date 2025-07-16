using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AADConnect.Controllers
{
    [Authorize]
    public class ResultsController : Controller
    {
        public async Task<IActionResult> Tokens()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            ViewData["IdToken"] = idToken ?? "";
            ViewData["AccessToken"] = accessToken ?? "";
            return View();
        }
    }
}
