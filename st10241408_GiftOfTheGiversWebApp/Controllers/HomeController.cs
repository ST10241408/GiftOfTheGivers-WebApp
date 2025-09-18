using ST10241408_GiftOfTheGiversWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ST10241408_GiftOfTheGiversWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ST10241408_GiftOfTheGiversWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            if (@User.Identity.IsAuthenticated)
            {
                var viewModel = new IncomingDataModel
                {
                    GoodsDonations = _context.GoodsDonation.ToList(),
                    MoneyDonations = _context.MoneyDonation.ToList(),
                    Disasters = _context.Disaster.ToList()
                };

                return View(viewModel);
            }
            else
            {
                return View();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

    }
}