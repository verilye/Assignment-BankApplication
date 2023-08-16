using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IDataAccessRepository
{
    bool CheckForPopulatedDb();
    bool CheckForAccount(int accountNumber);
    void InitUserData(List<Customer> data);
    Customer GetCustomerByCustomerId(int customerID);
    bool UpdateCustomer(Customer customer);
    Login GetLoginByCustomerId(int customerID);
    Login GetLoginByLoginId(string loginID);
    List<Account> GetAccountsByCustomerId(int customerID);
    void StoreTransaction(Transaction transaction);
    List<Transaction> GetTransactionsByAccountNumber(int accountNumber);
    bool UpdateLogin(Login login);
    List<BillPay> GetAllPendingBillPays();
    float GetAccountBalance(int accountNumber);
    void RemoveBillPay(BillPay billPay);
    void UpdateBillPay(BillPay billPay);
    void StoreBillPay(BillPay billPay);
    List<BillPay> GetBillPays(int accountNumber);

}



