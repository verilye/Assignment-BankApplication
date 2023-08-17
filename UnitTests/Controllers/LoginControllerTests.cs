using Microsoft.AspNetCore.Mvc;
using Moq;
using WebDevAss2.Controllers;
using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;
using Xunit;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebDevAss2.Tests
{
    public class LoginControllerTests
    {
        [Theory]
        [InlineData("validLoginId", "validPassword", true)]
        [InlineData("invalidLoginId", "invalidPassword", false)]
        public async Task SubmitForm_ValidAndInvalidLogin_RedirectsOrReturnsUnauthorized(string loginId, string password, bool isValid)
        {
            // Arrange
            var mockLoginRepository = new Mock<ILoginRepository>();
            mockLoginRepository.Setup(repo => repo.ValidateLoginDetails(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(isValid ? new Login { LoginId = "validLoginId", Locked = 0 } : null);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext);

            var mockAuth = new Mock<IAuthenticationService>();
            mockAuth.Setup(x => x.SignInAsync(mockHttpContext, It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns("someUrl");
            mockUrlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);

            var controller = new LoginController(mockLoginRepository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext
                },
                Url = mockUrlHelper.Object
            };

            // Assign the mock authentication service
            controller.HttpContext.RequestServices = new ServiceCollection()
                .AddScoped<IAuthenticationService>(_ => mockAuth.Object)
                .AddScoped<IUrlHelperFactory>(_ => mockUrlHelperFactory.Object)
                .BuildServiceProvider();

            // Simulate form data
            controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "loginID", loginId },
                { "password", password }
            });

            // Act
            var result = await controller.SubmitForm();

            // Assert
            if (isValid)
            {
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Equal("Home", redirectToActionResult.ControllerName);
            }
            else
            {
                var statusCodeResult = Assert.IsType<ObjectResult>(result);
                Assert.Equal(401, statusCodeResult.StatusCode);
            }
        }
    }
}
