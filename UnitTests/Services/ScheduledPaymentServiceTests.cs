using System;
using System.Reflection;
using Moq;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;
using Xunit;


namespace YourNamespace.Tests.Services
{
    public class ScheduledPaymentServiceTests
    {
        [Fact]
        public void ProcessPaymentsAsync_Should_Process_Payments_When_Time_Passed_And_Not_Blocked()
        {
            // Arrange
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IPaymentRepository))).Returns(paymentRepositoryMock.Object);

            var paymentService = new ScheduledPaymentService(serviceProviderMock.Object);

            var nowUtc = DateTime.UtcNow;
            var pendingPayments = new List<BillPay>
            {
                new BillPay { ScheduleTimeUtc = nowUtc.AddMinutes(-30), Blocked = 0 },
                new BillPay { ScheduleTimeUtc = nowUtc.AddMinutes(-10), Blocked = 1 },
                new BillPay { ScheduleTimeUtc = nowUtc.AddMinutes(10), Blocked = 0 },
            };
            paymentRepositoryMock.Setup(repo => repo.GetPendingPayments()).Returns(pendingPayments);

            // Act
            InvokeProcessPaymentsAsync(paymentService, paymentRepositoryMock.Object);

            // Assert
            paymentRepositoryMock.Verify(repo => repo.CompletePayment(It.IsAny<BillPay>()), Times.Once());
        }

        private void InvokeProcessPaymentsAsync(ScheduledPaymentService paymentService, IPaymentRepository paymentRepository)
        {
            var processMethod = typeof(ScheduledPaymentService)
                .GetMethod("ProcessPaymentsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            processMethod.Invoke(paymentService, new object[] { paymentRepository });
        }

    }
}
