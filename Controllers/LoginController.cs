using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebDevAss2.Controllers;

public class LoginController : Controller
{
    private readonly ILoginRepository _loginRepository;
    public LoginController(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    public async Task<IActionResult> Index()
    {
         // If DB is unpopulated, populate it
        await _loginRepository.InitialiseDB();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitForm()
    {
        string loginID = Request.Form["loginID"]!; 
        string password = Request.Form["password"]!;
        Login? result = _loginRepository.ValidateLoginDetails(loginID, password);
        
        if (result != null && result.Locked == 0)
        {
            var claims = new List<Claim>
            {
                new Claim("LoginId", result.LoginId, ClaimValueTypes.String),
                new Claim("CustomerId", result.CustomerId.ToString(), ClaimValueTypes.String),
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            //Pass along error message here
            return StatusCode(401, "Invalid login");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
