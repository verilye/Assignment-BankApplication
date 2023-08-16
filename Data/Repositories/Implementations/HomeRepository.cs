using WebDevAss2.Models;
using SimpleHashing.Net;

namespace WebDevAss2.Data.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly IDataAccessRepository _dataAccess;

    public HomeRepository(IDataAccessRepository dataAccess)
    {
        _dataAccess = dataAccess;
      
    }

    public List<AccountViewModel> FetchAccounts(int customerID)
    {
        List<AccountViewModel> accountsWithBalance = new();
        List<Account> accounts = _dataAccess.GetAccountsByCustomerId(customerID);
        if (accounts != null)
        {
            foreach (var account in accounts)
            {
                List<Transaction> transactions = _dataAccess.GetTransactionsByAccountNumber(account.AccountNumber);

                float deposits = transactions!.Where(t => t.TransactionType == TransactionType.D
                   || (t.TransactionType == TransactionType.T && t.DestinationAccountNumber == null)).Sum(t => t.Amount);
                float withdraws = transactions!.Where(t => t.TransactionType == TransactionType.W
                    || (t.TransactionType == TransactionType.T && t.DestinationAccountNumber != null)).Sum(t => t.Amount);

                var accountViewModel = new AccountViewModel
                {
                    Account = account,
                    Balance = deposits - withdraws,
                    Transactions = transactions,
                };

                accountsWithBalance.Add(accountViewModel);
            }
        }
        return accountsWithBalance;
    }

    public void ValidateAndStoreTransaction(Transaction transaction)
    {
        _dataAccess.StoreTransaction(transaction);

        return;
    }

    public bool ConfirmDestinationAccountExists(int accountNumber)
    {

        return _dataAccess.CheckForAccount(accountNumber);
    }

    public Customer FetchCustomerById(int customerID)
    {

        return _dataAccess.GetCustomerByCustomerId(customerID);

    }

    public bool StoreCustomerDetails(Customer customer)
    {

        if (_dataAccess.UpdateCustomer(customer) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string HashPassword(string password)
    {
        ISimpleHash simpleHash = new SimpleHash();
        string hashedPassword = simpleHash.Compute(password);

        return hashedPassword;
    }

    public bool ChangePassword(Login login)
    {

        return _dataAccess.UpdateLogin(login);
    }



}