namespace WebDevAss2.Data.Repositories;

public interface ILoginRepository{

    bool ValidateLoginDetails(string username, string password);
    
};