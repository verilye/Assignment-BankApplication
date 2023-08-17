using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IPaymentRepository{

    void SeedPayees();
    List<BillPay> GetPendingPayments();
    float GetBalanceOfAccount(int accountId);
    void CompletePayment(BillPay payment);
    void MarkPaymentAsFailed(BillPay payment);
    void ValidateAndStoreBillPay(Transaction transaction);
    void AddNewBillPay(BillPay billPay);
    List<BillPay> GetBillPaysByAccountNumber(int accountNumber);
    bool ConfirmPayeeIdExists(int payeeId);
    BillPay GetBillPayById(int billPayId);
    
};