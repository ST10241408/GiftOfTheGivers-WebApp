using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp.Controllers;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;
using ST10241408_GiftOfTheGiversWebApp.Tests.Helpers;
using Xunit;

namespace ST10241408_GiftOfTheGiversWebApp.Tests.Controllers
{
    public class UserMoneyDonationsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserMoneyDonationsController _controller;

        public UserMoneyDonationsControllerTests()
        {
            _context = TestHelper.GetInMemoryDbContext();
            _controller = new UserMoneyDonationsController(_context);
        }

        [Fact]
        public async Task Create_FirstMoneyDonation_CreatesMoneyRecord()
        {
            // Arrange
            var username = "testuser";
            _controller.ControllerContext = TestHelper.CreateControllerContext(username);

            var moneyDonation = new MoneyDonation
            {
                DATE = DateTime.Now.Date,
                AMOUNT = 1000.50m,
                DONOR = "Anonymous"
            };

            // Act
            var result = await _controller.Create(moneyDonation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            // Verify money record was created
            var money = await _context.Money.FirstOrDefaultAsync();
            Assert.NotNull(money);
            Assert.Equal(1000.50m, money.TotalMoney);
            Assert.Equal(1000.50m, money.RemainingMoney);
        }

        [Fact]
        public async Task Create_ExistingMoneyRecord_UpdatesCorrectly()
        {
            // Arrange
            var username = "testuser";
            _controller.ControllerContext = TestHelper.CreateControllerContext(username);

            // Add existing money
            _context.Money.Add(new Money { TotalMoney = 5000, RemainingMoney = 3000 });
            await _context.SaveChangesAsync();

            var moneyDonation = new MoneyDonation
            {
                DATE = DateTime.Now.Date,
                AMOUNT = 2000m,
                DONOR = username
            };

            // Act
            var result = await _controller.Create(moneyDonation);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            // Verify money was updated
            var money = await _context.Money.FirstOrDefaultAsync();
            Assert.Equal(7000m, money.TotalMoney);
            Assert.Equal(5000m, money.RemainingMoney);
        }

        [Fact]
        public async Task Index_ReturnsUserMoneyDonations()
        {
            // Arrange
            var username = "testuser";
            _controller.ControllerContext = TestHelper.CreateControllerContext(username);

            // Add test data
            _context.MoneyDonation.Add(new MoneyDonation
            {
                USERNAME = username,
                DATE = DateTime.Now.Date,
                AMOUNT = 500m,
                DONOR = username
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MoneyDonation>>(viewResult.Model);
            Assert.Single(model);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}