using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ImageMagick;

namespace WebDevAss2.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IHomeRepository _homeRepository;
    private readonly IPaymentRepository _paymentRepository;

    public HomeController(IHomeRepository homeRepository, IPaymentRepository paymentRepository)
    {
        _homeRepository = homeRepository;
        _paymentRepository = paymentRepository;

    }

    public ActionResult Index()
    {
        int customerID = Int32.Parse(User.FindFirstValue("CustomerId")!)!;

        // Get a list of accounts to display in option menues
        List<AccountViewModel> accounts = _homeRepository.FetchAccounts(customerID);
        Customer customer = _homeRepository.FetchCustomerById(customerID);
        List<BillPay> billPays = new List<BillPay>();
        foreach (var accountViewModel in accounts)
        {
            List<BillPay> result = _paymentRepository.GetBillPaysByAccountNumber(accountViewModel.Account.AccountNumber);
            billPays.AddRange(result);
        }
        HomeViewDTO dto = new HomeViewDTO
        {
            AccountViewModels = accounts,
            Customer = customer,
            BillPays = billPays
        };

        return View(dto);
    }

    [HttpPost]
    public IActionResult Deposit([FromForm] Transaction transaction)
    {
        Console.WriteLine("HELLO WORLD");
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
            var validationErrors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

            // Return a 400 Bad Request status code along with validation errors
            return BadRequest(validationErrors);
        }

    }

    [HttpPost]
    public IActionResult SubmitProfile([FromForm] Customer customer)
    {
        bool result = false;
        if (ModelState.IsValid)
        {
            result = _homeRepository.StoreCustomerDetails(customer);
        }

        if (result)
        {
            // Process request
            return RedirectToAction("Index", "Home");
        }
        else
        {
            var validationErrors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

            // Return a 400 Bad Request status code along with validation errors
            return BadRequest(validationErrors);
        }

    }

    public IActionResult ChangePassword()
    {

        ClaimsPrincipal user = HttpContext.User;
        Claim customerIdClaim = user.FindFirst("CustomerId")!;
        Claim loginIdClaim = user.FindFirst("LoginId")!;
        int customerID = Int32.Parse(customerIdClaim.Value);
        string loginID = loginIdClaim.Value;

        string password = Request.Form["password"]!;
        string hashedPassword = _homeRepository.HashPassword(password);

        Login login = new Login
        {
            LoginId = loginID,
            CustomerId = customerID,
            PasswordHash = hashedPassword
        };

        if (_homeRepository.ChangePassword(login) && ModelState.IsValid)
        {
            // Process request
            return RedirectToAction("Index", "Home");
        }
        else
        {
            var validationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Return a 400 Bad Request status code along with validation errors
            return BadRequest(validationErrors);
        }
    }


    [HttpPost]
    public IActionResult BillPay([FromForm] BillPay billPay)
    {
        if (ModelState.IsValid && _paymentRepository.ConfirmPayeeIdExists(billPay.PayeeId))
        {
            billPay.Blocked = 0;
            _paymentRepository.AddNewBillPay(billPay);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            var validationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Return a 400 Bad Request status code along with validation errors
            return BadRequest(validationErrors);
        }
    }

    [HttpPost]
    public IActionResult RetryBillPay()
    {
        string billPayId = Request.Form["billPayId"]!;
        BillPay bill = _paymentRepository.GetBillPayById(Int32.Parse(billPayId));

        if (bill.Blocked == 1)
        {
            return BadRequest("BLOCKED");
        }
        else
        {
            // Get rid of failed billpay
            _paymentRepository.CompletePayment(bill);
            //retry billpay
            bill.BillPayId = 0;
            bill.Failed = false;
            _paymentRepository.AddNewBillPay(bill);


            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public IActionResult CancelBillPay()
    {
        string billPayId = Request.Form["billPayId"]!;
        BillPay bill = _paymentRepository.GetBillPayById(Int32.Parse(billPayId));

        _paymentRepository.CompletePayment(bill);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfilePicture(IFormFile profilePictureFile)
    {
        if (profilePictureFile != null && profilePictureFile.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            profilePictureFile.CopyTo(memoryStream);

            //Get customer from db, add profilePicture file to ProfilePicture, and GET THE HELL OUT 

            int customerID = Int32.Parse(User.FindFirstValue("CustomerId")!)!;
            Customer customer = _homeRepository.FetchCustomerById(customerID);

            if (customer != null)
            {
                // Convert the image to JPG format using Magick.NET
                using (var imageStream = new MemoryStream(memoryStream.ToArray()))
                {
                    using (var image = new MagickImage(imageStream))
                    {
                        // Resize the image to 400x400 pixels
                        image.Resize(new MagickGeometry(400, 400));
                        // Set the format to JPG
                        image.Format = MagickFormat.Jpg;

                        // Convert the image to bytes and update the customer's profile picture
                        customer.ProfilePicture = image.ToByteArray();
                    }
                }
                _homeRepository.StoreCustomerDetails(customer);
            }
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult GetProfilePicture(string userId)
    {
        // Get customer 
        int customerID = Int32.Parse(User.FindFirstValue("CustomerId")!)!;
        Customer customer = _homeRepository.FetchCustomerById(customerID);

        if (customer?.ProfilePicture != null)
        {
            return File(customer.ProfilePicture, "image/jpeg"); // Adjust content type as needed
        }
        else
        {
            // Return a default image or placeholder
            var defaultImagePath = "path_to_default_image.jpg";
            var defaultImageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
            return File(defaultImageBytes, "image/jpeg");
        }
    }

    [HttpPost]
    public IActionResult DeleteProfilePicture()
    {
        int customerID = Int32.Parse(User.FindFirstValue("CustomerId")!)!;
        Customer customer = _homeRepository.FetchCustomerById(customerID);

        if (customer != null)
        {
            customer.ProfilePicture = null;
            _homeRepository.StoreCustomerDetails(customer);
        }

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
