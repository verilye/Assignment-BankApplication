using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;

namespace WebDevAss2.Controllers;

public class LoginController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitForm(){

        string customerID = Request.Form["customerID"]!;
        string password = Request.Form["password"]!;

        Console.WriteLine("Im alive");
        Console.WriteLine(customerID +" "+ password);
        return View("Index");

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
