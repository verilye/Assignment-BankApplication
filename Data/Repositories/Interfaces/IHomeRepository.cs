using WebDevAss2.Models;
namespace WebDevAss2.Data.Repositories;

public interface IHomeRepository{

   List<AccountViewModel> FetchAccounts(int customerID);
   bool ValidateAndStoreTransaction(Transaction transaction);
   bool ConfirmDestinationAccountExists(int accountNumber);
   Customer FetchCustomerById(int customerId);
   bool StoreCustomerDetails(Customer customer);
   string HashPassword(string password);
   bool ChangePassword(Login password);
    
};