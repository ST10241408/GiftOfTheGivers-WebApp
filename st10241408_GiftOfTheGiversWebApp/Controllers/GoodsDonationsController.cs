using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10241408_GiftOfTheGiversWebApp;
using ST10241408_GiftOfTheGiversWebApp.Data;
using ST10241408_GiftOfTheGiversWebApp.Models;

namespace ST10241408_GiftOfTheGiversWebApp.Controllers
{
    public class GoodsDonationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoodsDonationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoodsDonations
        public async Task<IActionResult> Index()
        {
            try
            {
                return _context.GoodsDonation != null ?
                            View(await _context.GoodsDonation.ToListAsync()) :
                            Problem("Entity set 'ApplicationDbContext.GoodsDonation'  is null.");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Index: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving goods donations.");
            }
        }

        // GET: GoodsDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.GoodsDonation == null)
                {
                    return NotFound();
                }

                var goodsDonation = await _context.GoodsDonation
                    .FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
                if (goodsDonation == null)
                {
                    return NotFound();
                }

                return View(goodsDonation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Details: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving goods donation details.");
            }
        }

        // GET: UserGoodsDonations/Create
        [Authorize]
        public IActionResult Create()
        {
            try
            {
                // Get the unique categories for the logged-in user, excluding "Cloths" and "Non-Perishable Foods"
                var existingCategories = _context.GoodsDonation
                    .Where(d => d.USERNAME == User.Identity.Name && d.CATEGORY != "Cloths" && d.CATEGORY != "Non-Perishable Foods")
                    .Select(d => d.CATEGORY)
                    .Distinct()
                    .ToList();

                ViewBag.CategoryList = existingCategories;

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, "An error occurred while creating a new goods donation.");
            }
        }

        // POST: UserGoodsDonations/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GOODS_DONATION_ID, USERNAME, DATE, ITEM_COUNT, CATEGORY, DESCRIPTION, DONOR")] GoodsDonation goodsDonation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (goodsDonation.DATE < DateTime.Now.Date)
                    {
                        ModelState.AddModelError("DATE", "Date cannot be earlier than today.");
                        return View(goodsDonation);
                    }

                    // Check if the user selected "Anonymous" as the donor
                    if (goodsDonation.DONOR == "Anonymous")
                    {
                        goodsDonation.DONOR = "Anonymous";
                    }
                    else
                    {
                        // Set DONOR to the current logged-in user's username
                        var currentUser = User.Identity?.Name;
                        goodsDonation.DONOR = currentUser;
                    }

                    // Check if the category exists in the GoodsInventory
                    var inventoryItem = _context.GoodsInventory.FirstOrDefault(g => g.CATEGORY == goodsDonation.CATEGORY);

                    if (inventoryItem != null)
                    {
                        // Update the item count in the existing record
                        inventoryItem.ITEM_COUNT += goodsDonation.ITEM_COUNT;
                    }
                    else
                    {
                        // Create a new record in the GoodsInventory
                        _context.GoodsInventory.Add(new GoodsInventory
                        {
                            CATEGORY = goodsDonation.CATEGORY, // Corrected the property name
                            ITEM_COUNT = goodsDonation.ITEM_COUNT
                        });
                    }

                    // Check if the category already exists in the user's previous donations
                    var existingCategories = _context.GoodsDonation
                        .Where(d => d.USERNAME == goodsDonation.USERNAME && d.CATEGORY != "Cloths" && d.CATEGORY != "Non-Perishable Foods")
                        .Select(d => d.CATEGORY)
                        .Distinct()
                        .ToList();

                    if (!existingCategories.Contains(goodsDonation.CATEGORY))
                    {
 // Add the new category to the user's previous donations
                        _context.GoodsDonation.Add(goodsDonation);
                    }
                    else
                    {
                        // Update the existing record
                        var existingDonation = _context.GoodsDonation.FirstOrDefault(d => d.USERNAME == goodsDonation.USERNAME && d.CATEGORY == goodsDonation.CATEGORY);
                        existingDonation.ITEM_COUNT += goodsDonation.ITEM_COUNT;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(goodsDonation);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Create POST: {ex.Message}");
                return StatusCode(500, "An error occurred while creating a new goods donation.");
            }
        }

        // GET: GoodsDonations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || _context.GoodsDonation == null)
                {
                    return NotFound();
                }

                var goodsDonation = await _context.GoodsDonation.FindAsync(id);
                if (goodsDonation == null)
                {
                    return NotFound();
                }

                return View(goodsDonation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Edit: {ex.Message}");
                return StatusCode(500, "An error occurred while editing a goods donation.");
            }
        }

        // POST: GoodsDonations/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GOODS_DONATION_ID, USERNAME, DATE, ITEM_COUNT, CATEGORY, DESCRIPTION, DONOR")] GoodsDonation goodsDonation)
        {
            try
            {
                if (id != goodsDonation.GOODS_DONATION_ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(goodsDonation);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GoodsDonationExists(goodsDonation.GOODS_DONATION_ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(goodsDonation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Edit POST: {ex.Message}");
                return StatusCode(500, "An error occurred while editing a goods donation.");
            }
        }

        // GET: GoodsDonations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.GoodsDonation == null)
                {
                    return NotFound();
                }

                var goodsDonation = await _context.GoodsDonation
                    .FirstOrDefaultAsync(m => m.GOODS_DONATION_ID == id);
                if (goodsDonation == null)
                {
                    return NotFound();
                }

                return View(goodsDonation);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting a goods donation.");
            }
        }

        // POST: GoodsDonations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.GoodsDonation == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.GoodsDonation'  is null.");
                }
                var goodsDonation = await _context.GoodsDonation.FindAsync(id);
                if (goodsDonation != null)
                {
                    _context.GoodsDonation.Remove(goodsDonation);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteConfirmed: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting a goods donation.");
            }
        }

        private bool GoodsDonationExists(int id)
        {
            try
            {
                return _context.GoodsDonation.Any(e => e.GOODS_DONATION_ID == id);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GoodsDonationExists: {ex.Message}");
                return false;
            }
        }
    }
}