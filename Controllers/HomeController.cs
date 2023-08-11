using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;


namespace WebDevAss2.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IHomeRepository _homeRepository;

    public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
    {
        _logger = logger;
        _homeRepository = homeRepository;
        
    }

    [HttpPost]
    public IActionResult Deposit([FromForm]Transaction transaction)
    {

        // _homeRepository.ValidateAndStoreTransaction(transaction);        

        return RedirectToAction("Index","Home");
    }

    public ActionResult Index()
    {
        // If DB is unpopulated, populate it
        _homeRepository.InitialiseDB();
        // Get a list of accounts to display in option menues
        List<Account> accounts = _homeRepository.FetchAccounts();

        return View(accounts);
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
