using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ST10241408_GiftOfTheGiversWebApp.Tests
{
    public class SimpleTest
    {
        [Fact]
        public void SimpleMathTest()
        {
            // Arrange
            int a = 5;
            int b = 3;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void AnotherSimpleTest()
        {
            // Arrange
            string expected = "Hello World";

            // Act
            string actual = "Hello " + "World";

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BooleanTest()
        {
            // Arrange & Act
            bool isTrue = true;

            // Assert
            Assert.True(isTrue);
        }
    }
}