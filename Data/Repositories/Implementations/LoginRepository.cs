using SimpleHashing.Net;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;

namespace WebDevAss2.Data.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly IDataAccessRepository _dataAccess;

    public LoginRepository(IDataAccessRepository dataAccess)
    {
        _dataAccess = dataAccess;
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