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

    public bool CheckForAccount(int accountNumber){
        return _context.Accounts.Any(a => a.AccountNumber == accountNumber);
         
    }
    public void InitUserData(List<Customer> data)
    {
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

    public Customer GetCustomerByCustomerId(int customerID){
        return _context.Customers
            .FirstOrDefault(u=>u.CustomerId == customerID)!;
    }

    public bool UpdateCustomer(Customer customer){
        
        var result = _context.Customers.Find(customer.CustomerId);
        
        if(result != null){
            _context.Entry(result).CurrentValues.SetValues(customer);
            _context.SaveChanges();
            return true;
        }else{
            return false;
        }
    }

    public Login GetLoginByCustomerId(int customerID){
        return _context.Logins
            .FirstOrDefault(u=>u.CustomerId == customerID)!;
    }

    public void StoreTransaction(Transaction transaction)
    {
        _context.Transactions
        .Add(transaction);
        _context.SaveChanges();
    }

    public List<Account> GetAccountsByCustomerId(int customerID)
    {
        return _context.Accounts
        .Where(u=>u.CustomerId == customerID)
        .ToList();
    }

    public List<Transaction> GetTransactionsByAccountNumber(int accountNumber){

        return _context.Transactions
            .Where(u=>u.AccountNumber == accountNumber)
            .ToList();
    }
}