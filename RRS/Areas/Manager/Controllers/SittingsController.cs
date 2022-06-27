#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRS.Areas.Manager.Models;
using RRS.Data;
using System.Diagnostics;

namespace RRS.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class SittingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SittingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manager/Sittings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sittings.Include(s => s.Restaurant).Include(s => s.SittingType).OrderBy(s => s.Start);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Manager/Sittings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.SittingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }


        // GET: Manager/Sittings/Create
        public async Task<IActionResult> Create()
        {

            var m = new SittingVm()
            {
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description"),
                Start = DateTime.Today,
                End = DateTime.Today.AddHours(1),
                RestaurantId = 1,
                Days = new bool[7],
            };
            Debug.WriteLine("Sitting Create has been selected");
            //m.Days[1] = true; 
            return View(m);
        }
        // POST: Manager/Sittings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SittingVm m)
        {
            Debug.Write($"Debugging in Sitting Creator. The Resturant {m.Name} was selected. Resturant Sitting of {m.SittingTypes} was selected");
            var restaurant = await _context.Restaurants.Include(r => r.Sittings).FirstOrDefaultAsync(r => r.Id == m.RestaurantId);
            if (restaurant == null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!m.Repeat)
                    {
                        Debug.WriteLine($"");
                        restaurant.AddSitting(m.SittingTypeId, m.Name, m.Start, m.End, m.Capacity, m.IsPrivate, m.IsClosed);
                    }
                    else
                    {
                        // Peter int count = 0; //days from first start date, increments for every day repeated
                        for (int wi = 0; wi < m.NumberOfWeeks; wi++)  //wi = weekIndex, di = dayIndex
                        {
                            for (int di = 0; di < 7; di++)
                            {
                                // mine

                                if (m.Days[di] )
                                {
                                    restaurant.AddSitting(m.SittingTypeId, m.Name, m.Start.AddDays((di - (int)m.Start.DayOfWeek)+ wi*7), m.End.AddDays((di - (int)m.Start.DayOfWeek)+ wi*7), m.Capacity, m.IsPrivate, m.IsClosed);
                                }
                              
                                //if (m.Days[di])
                                //{
                                //    restaurant.AddSitting(m.SittingTypeId, m.Name, m.Start.AddDays(count), m.End.AddDays(count), m.Capacity, m.IsPrivate, m.IsClosed);
                                //}
                                // Peter   count++;
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Error", "OOPS, an unknown error has occurred");
                }
            }
            m.SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description");
            return View(m);
        }

        // GET: Manager/Sittings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings.FindAsync(id);
            if (sitting == null)
            {
                return NotFound();
            }
            var sittingVm = new SittingVm()
            {
                SittingId = sitting.Id,
                Name = sitting.Name,
                Capacity = sitting.Capacity,
                IsPrivate = sitting.IsPrivate,
                IsClosed = sitting.IsClosed,
                RestaurantId = sitting.RestaurantId,
                SittingTypeId = sitting.SittingTypeId,
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description"),
                Start = sitting.Start,
                End = sitting.End,
            };
            return View(sittingVm);
            //ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", sitting.RestaurantId);
            //ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Id", sitting.SittingTypeId);
            //return View(sitting);
        }

        // POST: Manager/Sittings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DefaultDuration,Start,End,Capacity,IsPrivate,IsClosed,RestaurantId,SittingTypeId")] Sitting sitting)
        {
            if (id != sitting.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(sitting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingExists(sitting.Id))
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
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", sitting.RestaurantId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Id", sitting.SittingTypeId);
            return View(sitting);
        }

        // GET: Manager/Sittings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.SittingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // POST: Manager/Sittings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sitting = await _context.Sittings.FindAsync(id);
            _context.Sittings.Remove(sitting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(int id)
        {
            return _context.Sittings.Any(e => e.Id == id);
        }
    }
}
