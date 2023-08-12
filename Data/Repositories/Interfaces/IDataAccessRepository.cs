using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IDataAccessRepository
{
    public bool CheckForPopulatedDb();
    public bool CheckForAccount(int accountNumber);
    public void InitUserData(List<Customer> data);
    public Customer GetCustomerByCustomerId(int customerID);
    public Login GetLoginByCustomerId(int customerID);
    public List<Account> GetAccountsByCustomerId(int customerID);
    public void StoreTransaction(Transaction transaction);
    public List<Transaction> GetTransactionsByAccountNumber(int accountNumber);


}



