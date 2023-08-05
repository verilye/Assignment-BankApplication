using SimpleHashing.Net;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;

namespace WebDevAss2.Data.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly IDataAccessRepository _dataAccess;

    public LoginRepository(IDataAccessRepository dataAccess){
        _dataAccess = dataAccess;
    }

    public bool ValidateLoginDetails(string username, string password)
    {
        if (username == null || password == null)
        {
            return false;
        }

        // get password from db
        Customer customer = _dataAccess.GetUserByCustomerId(Int32.Parse(username));

        Console.WriteLine(customer.CustomerId + " " + customer.Name + " " + customer.Address);

        // Retrieve algorithm used, number of iterations, salt and hash from PasswordHash db field 
        // (separated by $ characters and in the order listed here)
        // string[] hashInfo = hashedPassword.Split('$');
        // bool validated = new SimpleHash().Verify(password, hashedPassword);
        // bool validated = new SimpleHash().Verify(password, hashedPassword);
        return true;
    }

}