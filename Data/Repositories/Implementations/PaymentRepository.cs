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

    }

    public void MarkPaymentAsFailed(BillPay payment){

    }
}