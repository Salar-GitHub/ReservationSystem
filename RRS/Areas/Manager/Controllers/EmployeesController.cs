#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRS.Areas.Manager.Models;
using RRS.Data;

namespace RRS.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;


        }

        // GET: Manager/Employees
        public async Task<IActionResult> Index()
        {
            var m = await _context.Employees.Include(e => e.Restaurant).ToListAsync();
            return View(m);
        }

        // GET: Manager/Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var m = await _context.Employees
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (m == null)
            {
                return NotFound();
            }

            return View(m);
        }

        // GET: Manager/Employees/Create

        public async Task<IActionResult> Create()
        {
            var m = new CreateEmployeeVM()
            {
                RestaurantId = await _context.Restaurants.Select(r => r.Id).FirstOrDefaultAsync()
            };


            return View(m);
        }


        // POST: Manager/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeVM m)
        {


            if (ModelState.IsValid)
            {
                //var e = await _context.Employees.FirstOrDefaultAsync(e => e.Email == m.Email);
                //if (e == null)
                //{
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == m.Email);
                //var role = await _context.Roles
                //    .Where(r => r.Id == user.Id)
                //    .FirstOrDefaultAsync();
                
               

                //ToDo Check role not equal Employee to avoid duplication
                if (user == null )
                {
                    var newUser = new IdentityUser { UserName = m.Email, Email = m.Email };
                    var result = await _userManager.CreateAsync(newUser, "Abc123!@#");
                    newUser.EmailConfirmed = true;
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, "Employee");
                    }
                    var newEmployee = new Data.Employee
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Email = m.Email,
                        PhoneNumber = m.PhoneNumber,
                        RestaurantId = m.RestaurantId,
                        UserId = m.UserId,
                        TaxFileNumber = m.TaxFileNumber
                    };
                    _context.Employees.Add(newEmployee);
                }

                //}
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", m.RestaurantId);
                Console.WriteLine("Error invalid input");
                return View(m);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TaxFileNumber,Id,FirstName,LastName,Email,PhoneNumber,RestaurantId")] Data.Employee employee)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        var e = await _context.Employees.FirstOrDefaultAsync(e => e.Email == employee.Email);
        //        if (e == null)
        //        {
        //            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == employee.Email);
        //            if (user == null)
        //            {
        //                var newUser = new IdentityUser { UserName = employee.Email, Email = employee.Email };
        //                var result = await _userManager.CreateAsync(newUser, "Abc123!@#");
        //                newUser.EmailConfirmed = true;
        //                if (result.Succeeded)
        //                {
        //                    await _userManager.AddToRoleAsync(newUser, "Employee");
        //                }
        //                var newEmployee = new Data.Employee { 
        //                    FirstName = employee.FirstName,
        //                    LastName = employee.LastName,
        //                    Email = employee.Email,
        //                    PhoneNumber = employee.PhoneNumber,
        //                    RestaurantId = employee.RestaurantId,
        //                    UserId = employee.UserId,
        //                    TaxFileNumber = employee.TaxFileNumber
        //                };
        //                _context.Employees.Add(newEmployee);
        //            }

        //        }
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", employee.RestaurantId);
        //        Console.WriteLine("Error invalid input");
        //        return View(employee);
        //    }
        //}

        // GET: Manager/Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", employee.RestaurantId);

            return View(employee);
        }

        // POST: Manager/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxFileNumber,Id,FirstName,LastName,Email,PhoneNumber,RestaurantId,UserId")] Data.Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id", employee.RestaurantId);
            return View(employee);
        }

        // GET: Manager/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Manager/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
