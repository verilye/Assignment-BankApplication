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

    public List<Account> FetchAccounts(int customerID)
    {
        return _dataAccess.GetAccountsByCustomerId(customerID);
    }

    public async void InitialiseDB(){
        if (_dataAccess.CheckForPopulatedDb() == false)
        {
            List<Customer> customers = await _jsonDataWebService.FetchJsonData("https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/");
            _dataAccess.InitUserData(customers);
        }
    }

    

    public void ValidateAndStoreTransaction(Transaction transaction){

        // Add all business logic here

        _dataAccess.StoreTransaction(transaction);
        return;
    }

}