using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;

public class ScheduledPaymentService : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5); // Payment processing interval
    private readonly IServiceProvider _serviceProvider;

    public ScheduledPaymentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                await ProcessPaymentsAsync(paymentRepository);
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessPaymentsAsync(IPaymentRepository paymentRepository)
    {
        //Return all payments that HAVE NOT FAILED
        var pendingPayments = paymentRepository.GetPendingPayments();

        // MAKE SURE THAT THE TIME HAS PASSED

        foreach (var payment in pendingPayments)
        {
            if (payment.ScheduleTimeUtc < DateTime.UtcNow)
            {  // PROCESS PAYMENT - Turn into transaction and if successful return true
                bool paymentSuccessful = ProcessPayment(payment, paymentRepository);

                if (paymentSuccessful)
                {
                    paymentRepository.CompletePayment(payment);
                }
                else
                {
                    paymentRepository.MarkPaymentAsFailed(payment);
                }
            }
        }
    }

    private bool ProcessPayment(BillPay payment, IPaymentRepository paymentRepository)
    {
        float balance = paymentRepository.GetBalanceOfAccount(payment.AccountNumber);

        if (balance >= payment.Amount)
        {
            //CREATE TRANSACTION AND STORE
            Transaction transaction = new Transaction
            {
                TransactionID = 0,
                TransactionType = TransactionType.T,
                AccountNumber = payment.AccountNumber,
                DestinationAccountNumber = payment.PayeeId,
                Amount = payment.Amount,
                Comment = null,
                TransactionTimeUtc = payment.ScheduleTimeUtc,
            };
            paymentRepository.ValidateAndStoreBillPay(transaction);

            //PROPOGATE BILLPAY IF REOCCURING
            if (payment.Period == Period.M)
            {
                //create new billpay + 1 to month
                BillPay billPay = new BillPay
                {
                    BillPayId = 0,
                    AccountNumber = payment.AccountNumber,
                    PayeeId = payment.PayeeId,
                    Amount = payment.Amount,
                    ScheduleTimeUtc = payment.ScheduleTimeUtc = DateTime.UtcNow.AddMonths(1),
                    Period = payment.Period,
                    Failed = false,
                };

                paymentRepository.AddNewBillPay(billPay);
            }

            return true;

        }
        else
        {
            return false;
        }
    }
}
