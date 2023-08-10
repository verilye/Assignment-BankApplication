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

    public bool ValidateLoginDetails(string username, string password)
    {
        bool validated = false;
        Login? customerLogin = null;
        if (username.Length == 0 || password.Length == 0)
        {
            validated = false;
        }
        else
        {
           // get password from db
           customerLogin = _dataAccess.GetLoginByCustomerId(Int32.Parse(username));
        }

        if (customerLogin != null)
        {
            validated = new SimpleHash().Verify(password!, customerLogin.PasswordHash);
        }

        return validated;
    }

}