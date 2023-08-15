using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface ILoginRepository{

    Login? ValidateLoginDetails(string username, string password);
    
};