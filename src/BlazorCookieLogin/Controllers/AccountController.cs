using BlazorCookieLogin.Entity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace BlazorCookieLogin.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var user = await TestUser
            .Where(x => x.UserName == userName && x.Password == password)
            .Include(x => x.Role)
            .FirstAsync();

        if (user != null)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role!.Id.ToString()));
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(7)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
            user.Save();

            return Redirect("/");
        }
        ViewBag.State = "登录失败";
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Account/Login");
    }
}
