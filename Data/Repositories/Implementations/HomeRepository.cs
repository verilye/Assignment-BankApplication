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

    public bool ValidateAndStoreTransaction(Transaction transaction)
    {
        // Transfers cant be to the same account 
        // transactions must be above 0
        if(transaction.Amount <= 0 || transaction.AccountNumber == transaction.DestinationAccountNumber){
            return false;
        }

        // balance needs to be above 0 or 300 for a checking account
        float balance = _dataAccess.GetAccountBalance(transaction.AccountNumber);
        Account account = _dataAccess.GetAccountByNumber(transaction.AccountNumber);

        float charge = 0;
        if (transaction.TransactionType == TransactionType.T && transaction.DestinationAccountNumber == null)
        {
            charge = 0.1f;
        }
        else if (transaction.TransactionType == TransactionType.W)
        {
            charge = 0.05f;
        }

        if (account.AccountType == AccountType.C && transaction.TransactionType == TransactionType.W)
        {
              if (balance + 300 < transaction.Amount + charge)
            {
                return false;
            }
        }   
        else if(account.AccountType == AccountType.C && transaction.TransactionType == TransactionType.T)
        {
            if (balance + 300 < transaction.Amount + charge && transaction.DestinationAccountNumber !=null)
            {
                return false;
            }
        }
        else if(account.AccountType == AccountType.S && transaction.TransactionType == TransactionType.W)
        {
            if (balance < transaction.Amount + charge)
            {
                return false;
            }
        }
         else if(account.AccountType == AccountType.S && transaction.TransactionType == TransactionType.T)
        {
            if (balance < transaction.Amount + charge && transaction.DestinationAccountNumber !=null)
            {
                return false;
            }
        }

        // 2 free transactions if W or T with destination account
        List<Transaction> list = _dataAccess.GetTransactionsByAccountNumber(transaction.AccountNumber);

        int freeTransactions = 0;
        foreach (Transaction t in list)
        {
            if (t.TransactionType == TransactionType.S)
            {
                freeTransactions++;
            }
        }

        // apply service charges as an S type transaction
        if (freeTransactions >= 2)
        {

            Transaction serviceCharge = new Transaction
            {
                TransactionID = 0,
                TransactionType = TransactionType.S,
                AccountNumber = transaction.AccountNumber,
                DestinationAccountNumber = null,
                Amount = charge,
                Comment = " ",
                TransactionTimeUtc = transaction.TransactionTimeUtc,
            };

            _dataAccess.StoreTransaction(serviceCharge);
        }

        _dataAccess.StoreTransaction(transaction);

        return true;
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