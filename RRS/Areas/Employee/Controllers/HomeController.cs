using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;

namespace RRS.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            var restaurant = await _context.Restaurants
                                      .Include(r => r.Sittings.Where(s => s.Start.Day == DateTime.Now.Day))
                                      .ThenInclude(s => s.Reservations)
                                      .Include(r => r.Sittings)
                                      .ThenInclude(s => s.SittingType)
                                      .FirstAsync();

            return View(restaurant);
        }
    }
   
}
