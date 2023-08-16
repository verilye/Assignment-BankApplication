using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IPaymentRepository{

    List<BillPay> GetPendingPayments();
    void CompletePayment(BillPay payment);
    void MarkPaymentAsFailed(BillPay payment);
    
};