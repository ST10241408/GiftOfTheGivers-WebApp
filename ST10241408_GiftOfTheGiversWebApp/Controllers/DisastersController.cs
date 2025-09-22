using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;

namespace ST10241408_GiftOfTheGiversWebApp.Controllers
{
    public class DisastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Disasters
        public async Task<IActionResult> Index()
        {
            return _context.Disaster != null ?
                View(await _context.Disaster.ToListAsync()) :
                Problem("Entity set 'ApplicationDbContext.Disaster' is null.");
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster
                .FirstOrDefaultAsync(m => m.DISTATER_ID == id);

            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // GET: Disasters/Create
        [Authorize]
        public IActionResult Create()
        {
            var currentDate = DateTime.Now.Date;
            var tomorrow = currentDate.AddDays(1);

            var disaster = new Disaster
            {
                STARTDATE = currentDate,
                ENDDATE = tomorrow
            };

            return View(disaster);
        }

        // POST: Disasters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE,IsActive")] Disaster disaster)
        {
            if (ModelState.IsValid)
            {
                disaster.USERNAME = User.Identity.Name;

                if (disaster.STARTDATE < DateTime.Now.Date)
                {
                    ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                    return View(disaster);
                }

                disaster.IsActive = (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE) ? 1 : 0;

                if (disaster.ENDDATE < disaster.STARTDATE.Value.Date.AddDays(1))
                {
                    ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                    return View(disaster);
                }

                _context.Add(disaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(disaster);
        }

        // GET: Disasters/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // POST: Disasters/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DISTATER_ID,USERNAME,STARTDATE,ENDDATE,LOCATION,AID_TYPE")] Disaster disaster)
        {
            if (id != disaster.DISTATER_ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (disaster.STARTDATE < DateTime.Now.Date)
                    {
                        ModelState.AddModelError("STARTDATE", "Start date cannot be earlier than today.");
                        return View(disaster);
                    }

                    disaster.IsActive = (disaster.STARTDATE <= DateTime.Now.Date && DateTime.Now.Date <= disaster.ENDDATE) ? 1 : 0;

                    if (disaster.ENDDATE < disaster.STARTDATE.Value.Date.AddDays(1))
                    {
                        ModelState.AddModelError("ENDDATE", "End date must be at least one day after the start date.");
                        return View(disaster);
                    }

                    _context.Update(disaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterExists(disaster.DISTATER_ID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(disaster);
        }

        // GET: Disasters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disaster == null)
                return NotFound();

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disaster == null)
                return Problem("Entity set 'ApplicationDbContext.Disaster' is null.");

            var disaster = await _context.Disaster.FindAsync(id);
            if (disaster == null)
                return NotFound();

            _context.Disaster.Remove(disaster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterExists(int id)
        {
            return _context.Disaster.Any(e => e.DISTATER_ID == id);
        }
    }
}
