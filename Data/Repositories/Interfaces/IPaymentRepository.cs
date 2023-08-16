using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IPaymentRepository{

    List<BillPay> GetPendingPayments();
    float getBalanceOfAccount(int accountId);
    void CompletePayment(BillPay payment);
    void MarkPaymentAsFailed(BillPay payment);
    void ValidateAndStoreBillPay(Transaction transaction);
    void AddNewBillPay(BillPay billPay);
    List<BillPay> GetBillPaysByAccountNumber(int accountNumber);
    
};