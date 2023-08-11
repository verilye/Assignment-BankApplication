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

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitForm()
    {
        string customerID = Request.Form["customerID"]!;
        string password = Request.Form["password"]!;
        bool result = _loginRepository.ValidateLoginDetails(customerID, password);
        
        if (result == true)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customerID),
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
            return StatusCode(401, "Bad username or password");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
