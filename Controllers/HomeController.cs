using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;

namespace WebDevAss2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    IUserDataWebServiceRepository<List<Customer>> _jsonDataWebService;

    public HomeController(ILogger<HomeController> logger, IUserDataWebServiceRepository<List<Customer>> jsonDataWebService)
    {
        _logger = logger;
        _jsonDataWebService = jsonDataWebService;
    }

    public IActionResult Index()
    {
         // Paginate out user data here for proof of concept
        List<Customer> customers = _jsonDataWebService.FetchJsonData("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/").Result;

        return View(customers[0]);
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
