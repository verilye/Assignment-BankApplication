using SimpleHashing.Net;
using WebDevAss2.Models;
using WebDevAss2.Data.Repositories;

namespace WebDevAss2.Data.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IDataAccessRepository _dataAccess;

    public PaymentRepository(IDataAccessRepository dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public List<BillPay> GetPendingPayments(){

        return _dataAccess.GetAllPendingBillPays();
    }
    public void CompletePayment(BillPay payment){

        _dataAccess.RemoveBillPay(payment);
        return;

    }

    public void MarkPaymentAsFailed(BillPay payment){

        payment.Failed = true;

        _dataAccess.UpdateBillPay(payment);
    }

    public float getBalanceOfAccount(int accountId)
    {
        return _dataAccess.GetAccountBalance(accountId);
    }

    public void ValidateAndStoreBillPay(Transaction transaction)
    {
        _dataAccess.StoreTransaction(transaction);

        return;
    }

    public void AddNewBillPay(BillPay billPay){
        _dataAccess.StoreBillPay(billPay);
    }

    public List<BillPay> GetBillPaysByAccountNumber(int accountNumber){
        return _dataAccess.GetBillPays(accountNumber);
    }
}