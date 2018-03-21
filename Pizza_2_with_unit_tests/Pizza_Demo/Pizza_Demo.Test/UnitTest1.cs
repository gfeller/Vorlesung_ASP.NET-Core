using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pizza_Demo.Controllers;
using Pizza_Demo.Data;
using Pizza_Demo.Models;
using Xunit;

namespace Pizza_Demo.Test
{
    public class UnitTest1
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _dbContext;

        public UnitTest1()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Pizza"));

            var tempDataFactory = new Mock<ITempDataDictionaryFactory>();
            tempDataFactory.Setup(x => x.GetTempData(It.IsAny<HttpContext>())).Returns(new Mock<ITempDataDictionary>().Object);
            services.AddSingleton(tempDataFactory.Object);


            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<ApplicationDbContext>();
        }

        [Fact]
        public void CreateSimple()
        {
            OrderController controller = new OrderController(null, null);
            Assert.NotNull(controller.Create());
        }

        [Fact]
        public void CreateFailing()
        {
            var model = new NewOrderViewModel();
            var controller = new OrderController(null, null);
            ValidateModel(model, controller);


            var result = (StatusCodeResult)controller.Create(model);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }


        [Fact]
        public void CreateValid()
        {
            var model = new NewOrderViewModel() { Name = "Michael" };

            var controller = new OrderController(_dbContext, null);
            var context = new DefaultHttpContext { User = CreateUser(), RequestServices = _serviceProvider };
            controller.ControllerContext = new ControllerContext() { HttpContext = context };

            var view = controller.Create(model);
            Assert.IsType<PartialViewResult>(view);
        }

        private GenericPrincipal CreateUser()
        {
            string username = "username";
            string userid = Guid.NewGuid().ToString("N"); //could be a constant
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userid)
            };

            var genericIdentity = new GenericIdentity("");
            genericIdentity.AddClaims(claims);
            var genericPrincipal = new GenericPrincipal(genericIdentity, new[] { "Administrator" });
            return genericPrincipal;
        }




        private static void ValidateModel(object model, Controller controller)
        {
            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("CustomError", validationResult.ErrorMessage);
            }
        }



    }

    /// <summary>
    /// UserManager Fake
    /// </summary>
    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
        {

        }
    }


}
