using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            await SeedUsers();
            await SeedData(); 
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RedirectUser()
        {
            if(User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home", new { area = "Manager" }); 
            }
            else if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Home", new { area = "Employee" });
            }
            else if (User.IsInRole("Member"))
            {
                return RedirectToAction("Index", "Home", new { area = "Member" });
            }

            return RedirectToAction(nameof(Index)); 
        }


        public async Task<bool> SeedUsers()
        {
            //seed roles
            string[] roles = { "Member", "Employee", "Manager" };
            foreach (var r in roles)
            {
                if (!await _roleManager.RoleExistsAsync(r))
                {
                    await _roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            //seed users
            string[] usernames = { "member@e.com", "employee@e.com", "manager@e.com" };
            foreach (string username in usernames)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    user = new IdentityUser { UserName = username, Email = username, EmailConfirmed = true };
                    await _userManager.CreateAsync(user, "Aa123!@#");
                    switch (username)
                    {
                        case "member@e.com":
                            if (!(await _userManager.GetRolesAsync(user)).Any(r => r == "Member"))
                            {
                                await _userManager.AddToRoleAsync(user, "Member");
                            }
                            break;
                        case "employee@e.com":
                            if (!(await _userManager.GetRolesAsync(user)).Any(r => r == "Employee"))
                            {
                                await _userManager.AddToRoleAsync(user, "Employee");
                            }
                            break;
                        case "manager@e.com":
                            if (!(await _userManager.GetRolesAsync(user)).Any(r => r == "Employee"))
                            {
                                await _userManager.AddToRoleAsync(user, "Employee");
                            }
                            if (!(await _userManager.GetRolesAsync(user)).Any(r => r == "Manager"))
                            {
                                await _userManager.AddToRoleAsync(user, "Manager");
                            }
                            break;
                    }
                }
            }

            return true; 
        }

        public async Task<bool> SeedData()
        {

       

            //seed company
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == "Bean Scene"); 
            if(company == null)
            {
                company = new Company { Name = "Bean Scene", ABN = 1111111 };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync(); 
            }

            //seed restaurant
            var restaurant = await _context.Restaurants.Include(r=>r.Sittings).FirstOrDefaultAsync(r => r.Name == "Bean Scene - Petersham");
            if (restaurant == null)
            {
                restaurant = new Restaurant { Name = "Bean Scene - Petersham", Email = "bean.scene@e.com", Address = "123 Cafe Lane", PhoneNumber = "1111 1111"};
                company.Restaurants.Add(restaurant); 
                await _context.SaveChangesAsync();
            }
            //seed Areas 
            string[] Areas = { "Main", "Balcony", "Outside" };
            foreach (var area in Areas)
            {
                if (!await _context.Areas.AnyAsync(t => t.Name == area))
                {
                    _context.Areas.Add(new Area { Name = area,RestaurantId =1 });
                    await _context.SaveChangesAsync();
                }
            }
            //Seed Tables
            foreach (var area in Areas)
            {
                for (int i = 1; i < 11; i++)
                {
                    if (!await _context.Tables.AnyAsync(t => t.Name == $"{area} Table {i}"))
                    {
                        _context.Tables.Add(new Table { Name = $"{area} Table {i}", AreaId = _context.Areas.FirstOrDefault(a => a.Name == area).Id });
                        await _context.SaveChangesAsync();

                    }
                }
                
            }


            //seed sitting types
            string[] sittingTypes = { "Breakfast", "Lunch", "Dinner" }; 
            foreach(var st in sittingTypes)
            {
                if(!await _context.SittingTypes.AnyAsync(t => t.Description == st))
                {
                    _context.SittingTypes.Add(new SittingType { Description = st });
                    await _context.SaveChangesAsync(); 
                }
            }

            ////seed sittings
            //if(!restaurant.Sittings.Any())
            //{
            //    var today = DateTime.Now.Date; 
            //    for (int i = 0; i < 7; i++)
            //    {
            //        //add breakfast sitting for the day
            //        var b = new Sitting
            //        {
            //            Capacity = 50,
            //           // DefaultDuration = 60,
            //            IsClosed = false,
            //            IsPrivate = false,
            //            Name = "Breakfast",
            //            SittingType = await _context.SittingTypes.FirstAsync(t => t.Description == "Breakfast"),
            //            Start = today.AddDays(i).AddHours(7),
            //            End = today.AddDays(i).AddHours(11),
            //        };
            //        restaurant.Sittings.Add(b);

            //        //add lunch sitting for the day
            //        var l = new Sitting
            //        {
            //            Capacity = 50,
            //           // DefaultDuration = 60,
            //            IsClosed = false,
            //            IsPrivate = false,
            //            Name = "Lunch",
            //            SittingType = await _context.SittingTypes.FirstAsync(t => t.Description == "Lunch"),
            //            Start = today.AddDays(i).AddHours(11).AddMinutes(30),
            //            End = today.AddDays(i).AddHours(15),
            //        };
            //        restaurant.Sittings.Add(l);


            //        //add dinner sitting for the day
            //        var d = new Sitting
            //        {
            //            Capacity = 50,
            //          //  DefaultDuration = 60,
            //            IsClosed = false,
            //            IsPrivate = false,
            //            Name = "Dinner",
            //            SittingType = await _context.SittingTypes.FirstAsync(t => t.Description == "Dinner"),
            //            Start = today.AddDays(i).AddHours(16).AddMinutes(30),
            //            End = today.AddDays(i).AddHours(23),
            //        };
            //        restaurant.Sittings.Add(d);
            //        await _context.SaveChangesAsync();
            //    }
                
            //}
            //seed Reservation Origins
            string[] ReservationOrigins = { "In-person", "Email", "Phone-call", "Website", "MobileApp" };
            foreach (var res in ReservationOrigins)
            {
                if (!await _context.ReservationOrigins.AnyAsync(t => t.Description == res))
                {
                    _context.ReservationOrigins.Add(new ReservationOrigin { Description = res });
                    await _context.SaveChangesAsync();
                }
            }
            
            //seed Reservation Statues
            string[] ReservationStatuses = { "Pending", "Confirmed", "Seated", "Completed", "Cancelled" };
            foreach (var res in ReservationStatuses)
            {
                if (!await _context.ReservationStatuses.AnyAsync(r => r.Description == res))
                {
                    _context.ReservationStatuses.Add(new ReservationStatus { Description = res });
                    await _context.SaveChangesAsync();
                }
            }

          



            return true; 

        }







    }
}