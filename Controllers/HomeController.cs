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
        Customer customer = _homeRepository.FetchCustomerById(customerID);
        HomeViewDTO dto = new HomeViewDTO
        {
            AccountViewModels = accounts,
            Customer = customer,
        };

        return View(dto);
    }

    [HttpPost]
    public IActionResult Deposit([FromForm] Transaction transaction)
    {
        _homeRepository.ValidateAndStoreTransaction(transaction);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Withdraw([FromForm] Transaction transaction)
    {
        _homeRepository.ValidateAndStoreTransaction(transaction);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Transfer([FromForm] Transaction transaction)
    {

        if (transaction.DestinationAccountNumber == null)
        {
            return StatusCode(401, "DestinationAccount is null");
        }
        else if (_homeRepository.ConfirmDestinationAccountExists((int)transaction.DestinationAccountNumber))
        {
            Transaction destinationTransaction = new Transaction
            {
                TransactionID = transaction.TransactionID,
                TransactionType = TransactionType.T,
                AccountNumber = (int)transaction.DestinationAccountNumber!,
                DestinationAccountNumber = null,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                TransactionTimeUtc = transaction.TransactionTimeUtc,
            };

            _homeRepository.ValidateAndStoreTransaction(transaction);
            _homeRepository.ValidateAndStoreTransaction(destinationTransaction);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            //Pass along error message here
            return StatusCode(401, "Bad destination account");
        }

    }

    [HttpPost]
    public IActionResult SubmitProfile([FromForm] Customer customer)
    {
        Console.WriteLine(customer.Name, customer.Tfn, customer.Mobile);
        bool result = _homeRepository.StoreCustomerDetails(customer);
        if(result){
            // Process request
            return RedirectToAction("Index", "Home");
        }else{
            // Notify front end it hasnt gone to plan
            return StatusCode(401, "Bad customer object");
        }
        
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
