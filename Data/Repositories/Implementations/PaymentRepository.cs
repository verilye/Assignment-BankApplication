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

    public void SeedPayees()
    {
        Payee payee1 = new Payee
        {
            PayeeID = 411229,
            Name = "Telstra",
            Address = "Your Mum's house",
            City = "Constantinople",
            State = State.QLD,
            PostCode = "1234",
            Phone = "(04) 8745 6211"
        };
        Payee payee2 = new Payee
        {
            PayeeID = 12345,
            Name = "Optus",
            Address = "WoopWoop",
            City = "Atlantis",
            State = State.QLD,
            PostCode = "1234",
            Phone = "(04) 8745 6211"
        };
        Payee payee3 = new Payee
        {
            PayeeID = 54321,
            Name = "ElonMusk",
            Address = "Mars",
            City = "Crater",
            State = State.QLD,
            PostCode = "1234",
            Phone = "(04) 8745 6211"
        };

        _dataAccess.AddPayee(payee1);
        _dataAccess.AddPayee(payee2);
        _dataAccess.AddPayee(payee3);
    }

    public List<BillPay> GetPendingPayments()
    {
        return _dataAccess.GetAllPendingBillPays();
    }
    public void CompletePayment(BillPay payment)
    {

        _dataAccess.RemoveBillPay(payment);
        return;

    }

    public void MarkPaymentAsFailed(BillPay payment)
    {

        payment.Failed = true;

        _dataAccess.UpdateBillPay(payment);
    }

    public float GetBalanceOfAccount(int accountId)
    {
        return _dataAccess.GetAccountBalance(accountId);
    }

    public void ValidateAndStoreBillPay(Transaction transaction)
    {
        _dataAccess.StoreTransaction(transaction);

        return;
    }

    public void AddNewBillPay(BillPay billPay)
    {
        _dataAccess.StoreBillPay(billPay);
    }

    public List<BillPay> GetBillPaysByAccountNumber(int accountNumber)
    {
        return _dataAccess.GetBillPays(accountNumber);
    }
    public bool ConfirmPayeeIdExists(int payeeId){
        return _dataAccess.CheckForPayeeId(payeeId);
    }

    public BillPay GetBillPayById(int billPayId){
        return _dataAccess.GetBillPayById(billPayId);
    }
}