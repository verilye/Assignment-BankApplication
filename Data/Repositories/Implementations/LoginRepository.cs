using SimpleHashing.Net;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;

namespace WebDevAss2.Data.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly IDataAccessRepository _dataAccess;
    private readonly IUserDataWebServiceRepository<List<Customer>> _jsonDataWebService;
    private readonly IPaymentRepository _paymentRepository;

    public LoginRepository(IDataAccessRepository dataAccess, IUserDataWebServiceRepository<List<Customer>> jsonDataWebService
    , IPaymentRepository paymentRepository)
    {
        _dataAccess = dataAccess;
        _jsonDataWebService = jsonDataWebService;
        _paymentRepository = paymentRepository;
    }

    public async Task InitialiseDB()
    {
        if (_dataAccess.CheckForPopulatedDb() == false)
        {
            List<Customer> customers = await _jsonDataWebService.FetchJsonData("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
            _dataAccess.InitUserData(customers);
            _paymentRepository.SeedPayees();
        }
    }

    public Login? ValidateLoginDetails(string username, string password)
    {
        if (username.Length == 0 || password.Length == 0)
        {
            return null;
        }
        else
        {
            // get password from db
            Login customerLogin = _dataAccess.GetLoginByLoginId(username);
            bool validated = new SimpleHash().Verify(password!, customerLogin.PasswordHash);
            if (validated)
            {
                return customerLogin;
            }
        }

        return null;

    }

}