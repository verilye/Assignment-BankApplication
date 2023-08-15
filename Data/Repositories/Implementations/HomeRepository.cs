using SimpleHashing.Net;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Data;
using Microsoft.AspNetCore.Http;

namespace WebDevAss2.Data.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly IDataAccessRepository _dataAccess;
    private readonly IUserDataWebServiceRepository<List<Customer>> _jsonDataWebService;


    public HomeRepository(IDataAccessRepository dataAccess, IUserDataWebServiceRepository<List<Customer>> jsonDataWebService)
    {
        _dataAccess = dataAccess;
        _jsonDataWebService = jsonDataWebService;
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

    public async void InitialiseDB()
    {
        if (_dataAccess.CheckForPopulatedDb() == false)
        {
            List<Customer> customers = await _jsonDataWebService.FetchJsonData("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
            _dataAccess.InitUserData(customers);
        }
    }

    public void ValidateAndStoreTransaction(Transaction transaction)
    {
        _dataAccess.StoreTransaction(transaction);

        return;
    }

    public bool ConfirmDestinationAccountExists(int accountNumber){

        return _dataAccess.CheckForAccount(accountNumber);
    }

    public Customer FetchCustomerById(int customerID){

        return _dataAccess.GetCustomerByCustomerId(customerID);

    }

    public bool StoreCustomerDetails(Customer customer){
        
        if( _dataAccess.UpdateCustomer(customer) == true){
             return true;
        }else{
            return false;
        }
    }

}