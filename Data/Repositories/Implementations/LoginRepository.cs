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
        Login customerLogin = _dataAccess.GetLoginByCustomerId(Int32.Parse(username));

        bool validated = new SimpleHash().Verify(password, customerLogin.PasswordHash);
        
        return validated;
    }

}