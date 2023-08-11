using WebDevAss2.Models;
namespace WebDevAss2.Data.Repositories;

public interface IHomeRepository{

   void InitialiseDB();

   List<Account> FetchAccounts(int customerID);
   void ValidateAndStoreTransaction(Transaction transaction);
    
};