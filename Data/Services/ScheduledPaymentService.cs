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
        // payment processing logic here
        // Return true if payment was successful, otherwise return false

        // Check to see if payment valid

        // complete new transaction

        return true;
    }
}
