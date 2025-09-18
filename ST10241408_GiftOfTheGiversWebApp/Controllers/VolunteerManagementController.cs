using Microsoft.AspNetCore.Mvc;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this line for Entity Framework Core

namespace ST10241408_GiftOfTheGiversWebApp.Controllers
{
    public class VolunteerManagementController : Controller
    {
        // Private readonly field to store the database context
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public VolunteerManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VolunteerManagement
        // Retrieves a list of all volunteers from the database
        public async Task<IActionResult> Index()
        {
            return View(await _context.Volunteers.ToListAsync());
        }

        // GET: VolunteerManagement/Create
        // Returns a view for creating a new volunteer
        public IActionResult Create()
        {
            return View();
        }

        // POST: VolunteerManagement/Create
        // Creates a new volunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,PhoneNumber")] Volunteer volunteer)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Add the volunteer to the database
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the invalid model
            return View(volunteer);
        }

        // GET: VolunteerManagement/Edit/5
        // Retrieves a volunteer by ID from the database
        public async Task<IActionResult> Edit(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the volunteer from the database
            var volunteer = await _context.Volunteers.FindAsync(id);
            // Check if the volunteer is null
            if (volunteer == null)
            {
                return NotFound();
            }
            // Return the view with the volunteer data
            return View(volunteer);
        }

        // POST: VolunteerManagement/Edit/5
        // Updates an existing volunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VolunteerID,Name,Email,PhoneNumber,RegistrationDate")] Volunteer volunteer)
        {
            // Check if the ID does not match the volunteer ID
            if (id != volunteer.VolunteerID)
            {
                return NotFound();
            }

            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Update the volunteer in the database
                _context.Update(volunteer);
                await _context.SaveChangesAsync();
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the invalid model
            return View(volunteer);
        }

        // GET: VolunteerManagement/Details/5
        // Retrieves a volunteer by ID from the database
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the ID is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the volunteer from the database
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(m => m.VolunteerID == id);
            // Check if the volunteer is null
            if (volunteer == null)
            {
                return NotFound();
            }
            // Return the view with the volunteer data
            return View(volunteer);
        }
    }
}