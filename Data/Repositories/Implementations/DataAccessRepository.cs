using System;
using System.Net.Http;
using Newtonsoft.Json;
using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public class DataAccessRepository : IDataAccessRepository
{
    private readonly McbaDbContext _context;
    public DataAccessRepository(McbaDbContext context)
    {
        _context = context;
    }


    public bool CheckForPopulatedDb(){
        return _context.Customers.Any();

    }
    public void InitUserData(List<Customer> data)
    {
        //Copy the data into new objects that dont offend bloody EFCORE

        foreach (Customer customer in data)
        {

            _context.Customers.Add(customer);
            
            foreach(Account account in customer.Accounts!){
                account.CustomerId = customer.CustomerId;
                _context.Accounts.Add(account);
                foreach(Transaction transaction in account.Transactions!){
                    transaction.AccountNumber = account.AccountNumber;
                    _context.Transactions.Add(transaction);
                }
            }

            Login login = customer.Login!;
            login.CustomerId = customer.CustomerId;

            _context.Logins.Add(login);
        }

        _context.SaveChanges();

        return;
    }

    public Customer GetUserByCustomerId(int customerID){
        return _context.Customers
            .FirstOrDefault(u=>u.CustomerId == customerID)!;
    }

    public Login GetLoginByCustomerId(int customerID){
        return _context.Logins
            .FirstOrDefault(u=>u.CustomerId == customerID)!;
    }
}