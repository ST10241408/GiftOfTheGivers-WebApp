using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;
using System.Security.Claims;

namespace ST10241408_GiftOfTheGiversWebApp.Tests.Helpers
{
    public static class TestHelper
    {
        public static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            return context;
        }

        public static ControllerContext CreateControllerContext(string username, string role = "User")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        public static void InitializeDatabase(ApplicationDbContext context)
        {
            // Add sample disasters for testing
            context.Disaster.AddRange(
                new Disaster
                {
                    DISTATER_ID = 1,
                    USERNAME = "testuser",
                    STARTDATE = DateTime.Now,
                    ENDDATE = DateTime.Now.AddDays(1),
                    LOCATION = "Test Location",
                    AID_TYPE = "Food",
                    IsActive = 1
                },
                new Disaster
                {
                    DISTATER_ID = 2,
                    USERNAME = "testuser2",
                    STARTDATE = DateTime.Now,
                    ENDDATE = DateTime.Now.AddDays(2),
                    LOCATION = "Test Location 2",
                    AID_TYPE = "Shelter",
                    IsActive = 1
                }
            );

            context.SaveChanges();
        }

        public static void CleanupDatabase(ApplicationDbContext context)
        {
            context.Disaster.RemoveRange(context.Disaster);
            context.SaveChanges();
        }
    }
}