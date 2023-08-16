using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;

public class ScheduledPaymentService : BackgroundService
{
    private readonly IPaymentRepository _paymentRepository; // Replace with your actual repository
    private readonly TimeSpan _interval = TimeSpan.FromHours(1); // Payment processing interval

    public ScheduledPaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessPaymentsAsync();
            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessPaymentsAsync()
    {
        // Retrieve pending payments from the repository
        var pendingPayments = _paymentRepository.GetPendingPayments();

        foreach (var payment in pendingPayments)
        {
            // Process payment (check balance, execute payment, update status, etc.)
            bool paymentSuccessful = ProcessPayment(payment);

            if (paymentSuccessful)
            {
                _paymentRepository.CompletePayment(payment);
            }
            else
            {
                _paymentRepository.MarkPaymentAsFailed(payment);
            }
        }
    }

    private bool ProcessPayment(BillPay payment)
    {
        // Return true if payment was successful, otherwise return false 
        // Check Balance is above 0

        float balance = _paymentRepository.getBalanceOfAccount(payment.AccountNumber);

        if(balance >= payment.Amount){

            Transaction transaction = new Transaction{
                TransactionID = 0,
                TransactionType = TransactionType.T,
                AccountNumber = payment.AccountNumber,
                DestinationAccountNumber = payment.PayeeId,
                Amount = payment.Amount,
                Comment = null,
                TransactionTimeUtc = payment.ScheduleTimeUtc,
            };
            _paymentRepository.ValidateAndStoreBillPay(transaction);
            
            if(payment.Period == Period.M){
                //create new billpay + 1 to month
                BillPay billPay = new BillPay{
                    BillPayId = 0,
                    AccountNumber = payment.AccountNumber,
                    PayeeId = payment.PayeeId,
                    Amount = payment.Amount,
                    ScheduleTimeUtc = payment.ScheduleTimeUtc =DateTime.UtcNow.AddMonths(1),
                    Period = payment.Period,
                    Failed = false,
                };

                _paymentRepository.AddNewBillPay(billPay);
            }

            return true;

        }else{
            return false;
        }
    }
}
