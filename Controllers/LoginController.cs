using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data;
using WebDevAss2.Data.Repositories;
using Microsoft.AspNetCore.Http;

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
    public IActionResult SubmitForm()
    {
        string customerID = Request.Form["customerID"]!;
        string password = Request.Form["password"]!;

        bool result = _loginRepository.ValidateLoginDetails(customerID, password);

        if (result == true)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            //Pass along error message here
            return View("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
