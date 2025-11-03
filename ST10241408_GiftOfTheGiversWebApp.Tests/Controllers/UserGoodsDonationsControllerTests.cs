using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp.Controllers;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;
using ST10241408_GiftOfTheGiversWebApp.Tests.Helpers;
using Xunit;

namespace ST10241408_GiftOfTheGiversWebApp.Tests.Controllers
{
    public class UserGoodsDonationsControllerTests
    {
        private ApplicationDbContext GetFreshContext()
        {
            return TestHelper.GetInMemoryDbContext();
        }
        [Fact]
        public async Task Create_ValidGoodsDonation_UpdatesInventoryAndRedirects()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserGoodsDonationsController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var goodsDonation = new GoodsDonation
            {
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 10,
                CATEGORY = "Clothing",
                DESCRIPTION = "Winter clothes",
                DONOR = "Anonymous",
                USERNAME = username  // Make sure USERNAME is set
            };

            // Debug: Check if USERNAME is set before calling the controller
            Assert.Equal(username, goodsDonation.USERNAME);

            // Act
            var result = await controller.Create(goodsDonation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verify inventory was updated
            var inventoryItem = await context.GoodsInventory
                .FirstOrDefaultAsync(g => g.CATEGORY == "Clothing");
            Assert.NotNull(inventoryItem);
            Assert.Equal(10, inventoryItem.ITEM_COUNT);

            // Also verify the donation was saved with the correct USERNAME
            var savedDonation = await context.GoodsDonation
                .FirstOrDefaultAsync(g => g.CATEGORY == "Clothing");
            Assert.NotNull(savedDonation);
            Assert.Equal(username, savedDonation.USERNAME);
        }

        [Fact]
        public async Task Create_DateInPast_ReturnsViewWithError()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserGoodsDonationsController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var goodsDonation = new GoodsDonation
            {
                DATE = DateTime.Now.Date.AddDays(-1), // Past date
                ITEM_COUNT = 5,
                CATEGORY = "Test Category",
                DESCRIPTION = "Test Description",
                USERNAME = username  // Add USERNAME
            };

            // Act
            var result = await controller.Create(goodsDonation);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.True(controller.ModelState.ContainsKey("DATE"));
        }

        [Fact]
        public async Task Index_ReturnsUserGoodsDonations()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserGoodsDonationsController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            // Add test data
            context.GoodsDonation.Add(new GoodsDonation
            {
                USERNAME = username,
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 5,
                CATEGORY = "Food",
                DESCRIPTION = "Canned goods",
                DONOR = username
            });
            await context.SaveChangesAsync();

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<GoodsDonation>>(viewResult.Model);
            Assert.Single(model); // Should have one item
        }

        [Fact]
        public async Task Create_AnonymousDonor_SetsDonorToAnonymous()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserGoodsDonationsController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var goodsDonation = new GoodsDonation
            {
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 15,
                CATEGORY = "Books",
                DESCRIPTION = "Educational books",
                DONOR = "Anonymous",
                USERNAME = username
            };

            // Act
            var result = await controller.Create(goodsDonation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            // Verify the donation was saved with Anonymous donor
            var savedDonation = await context.GoodsDonation
                .FirstOrDefaultAsync(g => g.CATEGORY == "Books");
            Assert.NotNull(savedDonation);
            Assert.Equal("Anonymous", savedDonation.DONOR);
        }

        [Fact]
        public async Task Create_NamedDonor_SetsDonorToUsername()
        {
            // Arrange
            using var context = GetFreshContext();
            var controller = new UserGoodsDonationsController(context);

            var username = "testuser";
            controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var goodsDonation = new GoodsDonation
            {
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 8,
                CATEGORY = "Toys",
                DESCRIPTION = "Children's toys",
                DONOR = "Named Donor", // This should be overwritten to username
                USERNAME = username
            };

            // Act
            var result = await controller.Create(goodsDonation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            // Verify the donation uses username as donor (not "Named Donor")
            var savedDonation = await context.GoodsDonation
                .FirstOrDefaultAsync(g => g.CATEGORY == "Toys");
            Assert.NotNull(savedDonation);
            Assert.Equal(username, savedDonation.DONOR);
        }
    }
}