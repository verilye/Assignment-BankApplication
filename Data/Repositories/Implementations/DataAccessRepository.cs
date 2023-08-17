using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public class DataAccessRepository : IDataAccessRepository
{
    private readonly McbaDbContext _context;
    public DataAccessRepository(McbaDbContext context)
    {
        _context = context;
    }

    public bool CheckForPopulatedDb()
    {
        return _context.Customers.Any();

    }

    public bool CheckForAccount(int accountNumber)
    {
        return _context.Accounts.Any(a => a.AccountNumber == accountNumber);

    }
    public void InitUserData(List<Customer> data)
    {
        foreach (Customer customer in data)
        {
            _context.Customers.Add(customer);

            foreach (Account account in customer.Accounts ?? Enumerable.Empty<Account>())
            {
                account.CustomerId = customer.CustomerId;
                _context.Accounts.Add(account);
                foreach (Transaction transaction in account.Transactions ?? Enumerable.Empty<Transaction>())
                {
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

    public Customer GetCustomerByCustomerId(int customerID)
    {
        return _context.Customers
            .FirstOrDefault(u => u.CustomerId == customerID)!;
    }

    public bool UpdateCustomer(Customer customer)
    {

        var result = _context.Customers.Find(customer.CustomerId);

        if (result != null)
        {
            _context.Entry(result).CurrentValues.SetValues(customer);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Login GetLoginByCustomerId(int customerID)
    {
        return _context.Logins
            .FirstOrDefault(u => u.CustomerId == customerID)!;
    }

    public Login GetLoginByLoginId(string loginID)
    {
        return _context.Logins
            .FirstOrDefault(u => u.LoginId == loginID)!;
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
        .Where(u => u.CustomerId == customerID)
        .ToList();
    }

    public List<Transaction> GetTransactionsByAccountNumber(int accountNumber)
    {

        return _context.Transactions
            .Where(u => u.AccountNumber == accountNumber)
            .ToList();
    }

    public bool UpdateLogin(Login login)
    {
        var result = _context.Logins.Find(login.LoginId);

        if (result != null)
        {
            _context.Entry(result).CurrentValues.SetValues(login);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<BillPay> GetAllPendingBillPays()
    {
        return _context.BillPays
            .Where(u => u.Failed == false)
            .ToList();
    }

    public float GetAccountBalance(int accountNumber)
    {

        List<Transaction> transactions = _context.Transactions
        .Where(u => u.AccountNumber == accountNumber)
        .ToList();

        float balance = 0;

        foreach (var transaction in transactions)
        {
            if (transaction.TransactionType == TransactionType.D)
            {
                balance = balance + transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.W)
            {
                balance = balance - transaction.Amount;
            }
            else if (transaction.TransactionType == TransactionType.T && transaction.DestinationAccountNumber == null)
            {
                balance = balance + transaction.Amount;
            }
            else
            {
                balance = balance - transaction.Amount;
            }
        }

        return balance;
    }

    public void RemoveBillPay(BillPay billPay)
    {
        var entityToDelete = _context.BillPays.Find(billPay.BillPayId); // Replace with the appropriate entity type and identifier

        if (entityToDelete != null)
        {
            // Step 2: Mark the entity for deletion
            _context.BillPays.Remove(entityToDelete);

            // Step 3: Save changes to the database
            _context.SaveChanges();
        }
        return;
    }

    public void UpdateBillPay(BillPay billPay)
    {
        var result = _context.BillPays.Find(billPay.BillPayId);

        if (result != null)
        {
            _context.Entry(result).CurrentValues.SetValues(billPay);
            _context.SaveChanges();
        }
    }

    public void StoreBillPay(BillPay billPay)
    {

        _context.BillPays.Add(billPay);
        _context.SaveChanges();

    }

    public List<BillPay> GetBillPays(int accountNumber)
    {
        return _context.BillPays.Where(x => x.AccountNumber == accountNumber).ToList();
    }

    public void AddPayee(Payee payee)
    {
        _context.Payees.Add(payee);
        _context.SaveChanges();
    }

    public bool CheckForPayeeId(int payeeId)
    {
        return _context.Payees.Any(a => a.PayeeID == payeeId);
    }

    public BillPay GetBillPayById(int billPayId){
        return _context.BillPays.Find(billPayId);
    }
}