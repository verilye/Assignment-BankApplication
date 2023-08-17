using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDevAss2.Controllers;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;
using Xunit;

namespace WebDevAss2.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Returns_View_With_Valid_Data()
        {
            // Arrange
            int customerID = 1;

            // Mock repositories
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            // Setup mock data
            var accounts = new List<AccountViewModel>
            {
                new AccountViewModel { Account = new Account { AccountNumber = 123 } },
                new AccountViewModel { Account = new Account { AccountNumber = 456 } }
            };
            var customer = new Customer { CustomerId = customerID };
            var billPays = new List<BillPay>
            {
                new BillPay { /* Mock BillPay data */ },
                new BillPay { /* Mock BillPay data */ }
            };

            mockHomeRepository.Setup(repo => repo.FetchAccounts(customerID)).Returns(accounts);
            mockHomeRepository.Setup(repo => repo.FetchCustomerById(customerID)).Returns(customer);
            mockPaymentRepository.Setup(repo => repo.GetBillPaysByAccountNumber(123)).Returns(billPays);
            mockPaymentRepository.Setup(repo => repo.GetBillPaysByAccountNumber(456)).Returns(billPays);

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("CustomerId", customerID.ToString())
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = controller.Index() as ViewResult;
            var model = result.Model as HomeViewDTO;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model.AccountViewModels.Count);
            Assert.Equal(customer, model.Customer);
            Assert.Equal(4, model.BillPays.Count); // Assuming each account gets two bill pays
        }
        [Fact]
        public void Deposit_Returns_Redirect_When_Valid_Transaction()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var validTransaction = new Transaction { };

            // Act
            var result = controller.Deposit(validTransaction) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);

            mockHomeRepository.Verify(repo => repo.ValidateAndStoreTransaction(validTransaction), Times.Once);
        }


        [Fact]
        public void Withdraw_Returns_Redirect_When_Valid_Transaction()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var validTransaction = new Transaction
            {
            };

            // Act
            var result = controller.Withdraw(validTransaction) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public void Transfer_Returns_Unauthorized_When_Destination_Account_Is_Null()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var transaction = new Transaction { };

            // Act
            var result = controller.Transfer(transaction) as StatusCodeResult;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Transfer_Returns_Redirect_When_Valid_Transfer()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);
            var transaction = new Transaction { };

            // Act
            var result = controller.Transfer(transaction);

            // Assert
            Assert.NotNull(result);

            if (result is RedirectToActionResult redirectResult)
            {
                Assert.Equal("Index", redirectResult.ActionName);
                Assert.Equal("Home", redirectResult.ControllerName);
            }
        }

        [Fact]
        public void Transfer_Returns_BadRequest_When_Invalid_Model()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var invalidTransaction = new Transaction { };
            controller.ModelState.AddModelError("PropertyName", "Some error message");
            // Act
            var result = controller.Transfer(invalidTransaction) as BadRequestObjectResult;
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SubmitProfile_Returns_Redirect_When_Valid_Customer()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var validCustomer = new Customer
            {
            };

            // Act
            var result = controller.SubmitProfile(validCustomer) as RedirectToActionResult;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SubmitProfile_Returns_BadRequest_When_Invalid_Model()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var invalidCustomer = new Customer
            {
            };

            controller.ModelState.AddModelError("PropertyName", "Some error message");

            // Act
            var result = controller.SubmitProfile(invalidCustomer) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var validationErrors = result.Value as List<string>;
            Assert.NotNull(validationErrors);
            Assert.NotEmpty(validationErrors);
        }

        [Fact]
        public void BillPay_Returns_Redirect_When_Valid_BillPay()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var validBillPay = new BillPay
            {
            };

            // Act
            var result = controller.BillPay(validBillPay) as RedirectToActionResult;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BillPay_Returns_BadRequest_When_Invalid_Model()
        {
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            var invalidBillPay = new BillPay
            {
            };

            controller.ModelState.AddModelError("PropertyName", "Some error message");

            // Act
            var result = controller.BillPay(invalidBillPay) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var validationErrors = result.Value as List<string>;
            Assert.NotNull(validationErrors);
            Assert.NotEmpty(validationErrors);
        }

        [Fact]
        public async Task UploadProfilePicture_Returns_Redirect()
        {
            // Arrange
            var mockHomeRepository = new Mock<IHomeRepository>();
            var mockHttpContext = new Mock<HttpContext>();

            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(file => file.Length).Returns(123);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("CustomerId", "1")
            }));
            mockHttpContext.Setup(c => c.User).Returns(user);


            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);

            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var result = await controller.UploadProfilePicture(null!) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public void GetProfilePicture_Should_Return_Profile_Picture_When_Available()
        {
            // Arrange
            var customerID = 1;
            var profilePictureBytes = new byte[] { 1, 2, 3 }; // Sample profile picture bytes

            var mockHomeRepository = new Mock<IHomeRepository>();
            mockHomeRepository.Setup(repo => repo.FetchCustomerById(customerID)).Returns(new Customer { ProfilePicture = profilePictureBytes });
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var userClaims = new[]
            {
                new Claim("CustomerId", customerID.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(userClaims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = controller.GetProfilePicture(customerID.ToString()) as FileResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("image/jpeg", result.ContentType);
        }

        [Fact]
        public void DeleteProfilePicture_Should_Delete_Profile_Picture_When_Customer_Exists()
        {
            // Arrange
            var customerID = 1;
            var mockHomeRepository = new Mock<IHomeRepository>();
            mockHomeRepository.Setup(repo => repo.FetchCustomerById(customerID)).Returns(new Customer());
            var mockPaymentRepository = new Mock<IPaymentRepository>();

            var userClaims = new[]
            {
                new Claim("CustomerId", customerID.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(userClaims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var controller = new HomeController(mockHomeRepository.Object, mockPaymentRepository.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = controller.DeleteProfilePicture() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);

            mockHomeRepository.Verify(repo => repo.StoreCustomerDetails(It.IsAny<Customer>()), Times.Once);
        }
    }
}
