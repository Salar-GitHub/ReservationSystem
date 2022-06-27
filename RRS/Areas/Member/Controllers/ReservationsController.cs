using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRS.Data;

namespace RRS.Areas.Member.Controllers
{
    [Area("Member"),Authorize(Roles ="Member")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Member/Reservations
        public async Task<IActionResult> Index()
        {   
            var user = await _userManager.GetUserAsync(User);
            var customer = _context.Customers.FirstOrDefault(p => p.UserId == user.Id);

            var reservations = await _context.Reservations.Include(r => r.Customer)
                .Include(r => r.ReservationOrigin).Include(r => r.ReservationStatus).Include(r => r.Sitting)
                .Where(r => r.CustomerId == customer.Id)
                .ToListAsync(); 
            return View(reservations);
        }
    }
}
