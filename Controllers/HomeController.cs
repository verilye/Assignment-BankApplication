using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WebDevAss2.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IHomeRepository _homeRepository;

    public HomeController(IHomeRepository homeRepository)
    {
        _homeRepository = homeRepository;

    }

    public ActionResult Index()
    {
        ClaimsPrincipal user = HttpContext.User;
        int customerID = Int32.Parse(user.Identity!.Name!);

        // If DB is unpopulated, populate it
        _homeRepository.InitialiseDB();
        // Get a list of accounts to display in option menues
        List<AccountViewModel> accounts = _homeRepository.FetchAccounts(customerID);

        return View(accounts);

    }


    [HttpPost]
    public IActionResult Deposit([FromForm] Transaction transaction)
    {
        _homeRepository.ValidateAndStoreTransaction(transaction);        

        return RedirectToAction("Index", "Home");
    }

    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
