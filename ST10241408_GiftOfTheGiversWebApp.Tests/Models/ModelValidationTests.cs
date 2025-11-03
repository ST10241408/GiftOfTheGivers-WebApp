using System.ComponentModel.DataAnnotations;
using ST10241408_GiftOfTheGiversWebApp.Models;
using Xunit;

namespace ST10241408_GiftOfTheGiversWebApp.Tests.Models
{
    public class ModelValidationTests
    {
        [Fact]
        public void Disaster_ValidModel_PassesValidation()
        {
            // Arrange
            var disaster = new Disaster
            {
                STARTDATE = DateTime.Now.Date,
                ENDDATE = DateTime.Now.Date.AddDays(1),
                LOCATION = "Test Location",
                AID_TYPE = "Food"
            };

            var validationContext = new ValidationContext(disaster);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(disaster, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Disaster_MissingRequiredFields_FailsValidation()
        {
            // Arrange
            var disaster = new Disaster
            {
                // Missing required fields
            };

            var validationContext = new ValidationContext(disaster);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(disaster, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void GoodsDonation_ValidModel_PassesValidation()
        {
            // Arrange
            var goodsDonation = new GoodsDonation
            {
                DATE = DateTime.Now.Date,
                ITEM_COUNT = 10,
                CATEGORY = "Food",
                DESCRIPTION = "Test Description"
            };

            var validationContext = new ValidationContext(goodsDonation);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(goodsDonation, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void MoneyDonation_ValidModel_PassesValidation()
        {
            // Arrange
            var moneyDonation = new MoneyDonation
            {
                DATE = DateTime.Now.Date,
                AMOUNT = 100.50m
            };

            var validationContext = new ValidationContext(moneyDonation);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(moneyDonation, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }
}