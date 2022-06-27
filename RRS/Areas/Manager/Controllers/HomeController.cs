using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;

namespace RRS.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
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
