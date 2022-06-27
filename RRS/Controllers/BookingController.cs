using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models.Booking;
using RRS.Services;
using RRS.Utility;
using RRS.Areas.Identity.Pages.Account;




namespace RRS.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly CustomerService _customerService;
        private readonly UserManager<IdentityUser> _userManager;


        public BookingController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<BookingController> logger, CustomerService customerService)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _customerService = customerService;
        }
        //GET Booking/Inquiry
        public async Task<IActionResult> Inquiry()
        {
            var m = new InquiryVM()
            {
                SelectedDate = DateTime.Now,
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description"),
            };

            return View(m);
        }


        //GET Booking/Sitting
        public async Task<IActionResult> Sittings(int sittingTypeId, DateTime date)
        {

            var restaurant = await _context.Restaurants
                                       .Include(r => r.Sittings
                                       .Where(s => (s.Start.Date == date) && (s.End.TimeOfDay >= date.TimeOfDay)
                                       && s.IsPrivate == false
                                       && s.IsClosed == false
                                       && s.SittingTypeId == sittingTypeId))
                                       .ThenInclude(s => s.Reservations)
                                       .Include(r => r.Sittings)
                                       .ThenInclude(s => s.SittingType)
                                       .FirstOrDefaultAsync();


            if (restaurant.Sittings.Count == 0)
            {

                return RedirectToAction("Message");

            }


            return PartialView("_AllSitting", restaurant);

        }




        public async Task<IActionResult> AllSitting()
        {
            var restaurant = await _context.Restaurants
                                      .Include(r => r.Sittings.Where(s => s.Start >= DateTime.Now && s.IsPrivate == false && s.IsClosed == false))
                                      .ThenInclude(s => s.Reservations)
                                      .Include(r => r.Sittings)
                                      .ThenInclude(s => s.SittingType)                                     
                                      .FirstOrDefaultAsync();
            //var sittings = await _context.Sittings.Where(s => s.Start >= DateTime.Now && s.IsPrivate == false && s.IsClosed == false)
            //                             .Include(s => s.Reservations)
            //                             .Include(s => s.SittingType)
            //                             .OrderBy( s => s.Start)
            //                             .ToListAsync();




            if (restaurant.Sittings.Count == 0)
            {

                return RedirectToAction("Message");
            }

            return PartialView("_AllSitting", restaurant);

        }

        //GET Booking/ResInfo
        public async Task<IActionResult> ReservationInformation(int sittingId)
        {
            var sitting = await _context.Sittings
                                      .Include(s => s.Restaurant)
                                      .Include(s => s.SittingType)
                                      .Include(s => s.Reservations)
                                      .ThenInclude(b => b.Customer)
                                      .FirstAsync(s => s.Id == sittingId);
            var m = new ReservationInformationVM()
            {
                SittingId = sittingId,
                StartTime = Helper.GetTimeDropDown(sitting.Start, sitting.End, 30),
                Duration = Helper.GetTimeDropDown(),
                RestaurantId = sitting.RestaurantId,
                RestaurantName = sitting.Restaurant.Name,
                SittingName = sitting.Name,
                SittingTypeName = sitting.SittingType.Description,
                Start = sitting.Start,
                End = sitting.End,
                Guest = 1

            };
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                var user = await _userManager.GetUserAsync(User);
                var customer = _context.Customers.FirstOrDefault(p => p.UserId == user.Id);
                m.FirstName = customer.FirstName;
                m.LastName = customer.LastName;
                m.Email = customer.Email;
                m.PhoneNumber = customer.PhoneNumber;
            }

            return View(m);

        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ReservationInformationVM m)
        {
            var sitting = await _context.Sittings.Where(s => s.Id == m.SittingId)
                                                 .Include(s => s.Reservations)
                                                 .FirstAsync();
            if (m.Guest > sitting.Vacancies)
            {
                //return RedirectToAction("NotAvailable", sitting.Vacancies);
                ModelState.AddModelError(key: "Guest", errorMessage: $"Not enough vacancies ( Available seats: {sitting.Vacancies})");
            }

            if (!ModelState.IsValid)
            {
                m.StartTime = Helper.GetTimeDropDown(sitting.Start, sitting.End, 30);
                m.Duration = Helper.GetTimeDropDown();

                return View("ReservationInformation", m);
            }

            var customer = await _customerService.FindOrCreateCustomerAsync(m.FirstName, m.LastName, m.Email, m.PhoneNumber, m.RestaurantId);
            var r = new Reservation()
            {
                StartTime = DateTime.Parse(m.SelectedTime),
                Duration = int.Parse(m.SelectedDuration),
                SittingId = m.SittingId,
                Guest = m.Guest,
                Note = m.Note,
                CustomerId = customer.Id,
                ReservationOriginId = 4,
                ReservationStatusId = 1,
                ReservationCode = Guid.NewGuid().ToString("N")
            };
            sitting.Reservations.Add(r);
            await _context.SaveChangesAsync();



            return RedirectToAction("Confirmation", new { reservationId = r.Id });
        }


        //Get Booking/Confirmation
        public async Task<IActionResult> Confirmation(int reservationId)
        {
            var reservation = await _context.Reservations.Where(r => r.Id == reservationId)
                .Include(r => r.Customer)
                .FirstAsync();
            var sitting = await _context.Sittings.Where(s => s.Id == reservation.SittingId)
                .Include(s => s.SittingType)
                .FirstAsync();
            var restaurant = await _context.Restaurants.Where(r => r.Id == sitting.RestaurantId).FirstAsync();
            var m = new ConfirmationVM()
            {
                ReservationId = reservationId,
                ReservationCode = reservation.ReservationCode,
                RestaurantName = restaurant.Name,
                SittingName = sitting.Name,
                SittingTypeName = sitting.SittingType.Description,
                StartTime = reservation.StartTime,
                Start = sitting.Start,
                Duration = reservation.Duration,
                Guest = reservation.Guest,
                Note = reservation.Note,
                FirstName = reservation.Customer.FirstName,
                LastName = reservation.Customer.LastName,
                Email = reservation.Customer.Email,
                PhoneNumber = reservation.Customer.PhoneNumber,
                CustomerId = reservation.CustomerId
            };
            return View(m);
        }



        public IActionResult Message()
        {
            return View();
        }
        public IActionResult NotAvailable(int m)
        {
            return View(m);
        }

    }
}

