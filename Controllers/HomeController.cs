using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Data;
using Microsoft.AspNetCore.Http;

namespace WebDevAss2.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserDataWebServiceRepository<List<Customer>> _jsonDataWebService;
    private readonly IDataAccessRepository _dataAccess;

    public HomeController(ILogger<HomeController> logger, IUserDataWebServiceRepository<List<Customer>> jsonDataWebService, IDataAccessRepository dataAccess)
    {
        _logger = logger;
        _jsonDataWebService = jsonDataWebService;
        _dataAccess = dataAccess;
    }

    [HttpPost]
    public IActionResult Deposit(Transaction transaction)
    {

        Console.WriteLine("WE GOT AN OBJECT OVA HEAH");
       
        Console.WriteLine(transaction.Account);
        Console.WriteLine(transaction.Amount);  
        Console.WriteLine(transaction.Comment);

        return RedirectToAction("Index","Home");
    }

    public async Task<IActionResult> Index()
    {
        ViewData["DisplayConfirmationWindow"] = false;
        if (_dataAccess.CheckForPopulatedDb() == false)
        {
            List<Customer> customers = await _jsonDataWebService.FetchJsonData("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
            _dataAccess.InitUserData(customers);
        }

        Transaction transaction = new();
        return View(transaction);
    }

    public ActionResult Logout()
    {

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
