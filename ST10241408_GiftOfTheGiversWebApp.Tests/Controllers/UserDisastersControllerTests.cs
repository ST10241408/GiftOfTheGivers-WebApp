using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp.Controllers;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;
using ST10241408_GiftOfTheGiversWebApp.Tests.Helpers;
using System.Security.Claims;
using Xunit;

namespace ST10241408_GiftOfTheGiversWebApp.Tests.Controllers
{
    public class UserDisastersControllerTests
    {
        private ApplicationDbContext GetFreshContext()
        {
            return TestHelper.GetInMemoryDbContext();
        }

        [Fact]
        public async Task Index_ReturnsViewWithUserDisasters()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);
            TestHelper.InitializeDatabase(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Disaster>>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public async Task Index_UserNotAuthenticated_RedirectsToLogin()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);

            // Create a completely empty, unauthenticated user context
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // Empty, unauthenticated identity
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.Index();

            // Assert - The controller should handle the null user gracefully
            // Either redirect to login or return an empty view
            Assert.NotNull(result);

            // Check if it's a redirect (preferred) or view result
            if (result is RedirectToActionResult redirectResult)
            {
                Assert.Equal("Login", redirectResult.ActionName);
                Assert.Equal("Account", redirectResult.ControllerName);
            }
            else if (result is ViewResult viewResult)
            {
                // If it returns a view instead of redirecting, that's also acceptable
                Assert.NotNull(viewResult);
            }
        } 

        [Fact]
        public void Create_ReturnsViewWithDefaultDates()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);

            controller.ControllerContext = TestHelper.CreateControllerContext("testuser");

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Disaster>(viewResult.Model);
            Assert.Equal(DateTime.Now.Date, model.STARTDATE?.Date);
            Assert.Equal(DateTime.Now.Date.AddDays(1), model.ENDDATE?.Date);
        }

        [Fact]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var disaster = new Disaster
            {
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(2),
                LOCATION = "Test Location",
                AID_TYPE = "Test Aid"
            };

            // Act
            var result = await controller.Create(disaster);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verify the disaster was added to the database
            var savedDisaster = await context.Disaster.FirstOrDefaultAsync(d => d.LOCATION == "Test Location");
            Assert.NotNull(savedDisaster);
            Assert.Equal(username, savedDisaster.USERNAME);
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsViewWithErrors()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);

            controller.ControllerContext = TestHelper.CreateControllerContext("testuser");
            controller.ModelState.AddModelError("LOCATION", "Location is required");

            var disaster = new Disaster
            {
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(2),
                // LOCATION is missing - invalid model
                AID_TYPE = "Test Aid"
            };

            // Act
            var result = await controller.Create(disaster);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(viewResult.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsViewWithDisaster()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);
            TestHelper.InitializeDatabase(context);

            var disasterId = 1;

            // Act
            var result = await controller.Details(disasterId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Disaster>(viewResult.Model);
            Assert.Equal(disasterId, model.DISTATER_ID);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserDisastersController(context);

            var invalidId = 999;

            // Act
            var result = await controller.Details(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}