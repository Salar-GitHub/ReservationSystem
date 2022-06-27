#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RRS.Areas.Employee.Models;
using RRS.Data;
using RRS.Models.Booking;
using RRS.Services;
using RRS.Utility;
using ReservationInformationVM = RRS.Areas.Employee.Models.ReservationInformationVM;

namespace RRS.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]

    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerService _customerService;


        public ReservationsController(ApplicationDbContext context, CustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        //Get/Employee/Reservations
        public async Task<IActionResult> Index(string StatusDescription, string id, string date)
        {
            var reservation = from r in _context.Reservations.Include(r => r.Customer)
                               .Include(r => r.ReservationOrigin)
                               .Include(r => r.ReservationStatus)
                               .Include(r => r.Sitting)
                               .OrderBy(r => r.StartTime)
                              select r;
            if (!string.IsNullOrEmpty(id))
            {
                reservation = reservation.Where(r => r.Customer.FirstName.Contains(id) || r.Customer.LastName.Contains(id));
            }
            if (!string.IsNullOrEmpty(StatusDescription))
            {
                reservation = reservation.Where(r => r.ReservationStatus.Id.ToString().Contains(StatusDescription));
            }
            if (!string.IsNullOrEmpty(date))
            {
                reservation = reservation.Where(r => r.Sitting.Start.ToString().Contains(date));
            }

            var m = new FilterVm()
            {
                ReservationStatuses = new SelectList(await _context.ReservationStatuses.ToListAsync(), "Id", "Description"),
                Reservations = await reservation.ToListAsync()
            };


            return View(m);
        }
        //GET: Empolyee/Reservations/Sittings

        public async Task<IActionResult> Sittings(string Description, string date)
        {
            var sittings = from s in _context.Sittings
                               .Include(s => s.SittingType)
                               .Include(s => s.Reservations)
                               .Include(s => s.Restaurant)
                               .Where(s => s.Start > DateTime.Now)
                               .OrderBy(s => s.Start)
                           select s;
            if (!string.IsNullOrEmpty(Description))
            {
                sittings = sittings.Where(s => s.SittingType.Id.ToString().Contains(Description));
            }
            if (!string.IsNullOrEmpty(date))
            {
                sittings = sittings.Where(s => s.Start.ToString().Contains(date));
            }
            var m = new SittingsVM()
            {
                Sittings = await sittings.ToListAsync(),
                SittingTypes = new SelectList(await _context.SittingTypes.ToListAsync(), "Id", "Description")
            };
            return View(m);
        }



        //Get: Employee/Reservation/ReservationInformation
        public async Task<ActionResult> ReservationInformation(int sittingId)
        {
            var tables = await _context.Tables.Where(t => !t.Reservations.Any(r => r.SittingId == sittingId)).ToListAsync();
            var sitiing = await _context.Sittings
                                  .Include(s => s.Restaurant)
                                  .Include(s => s.SittingType)
                                  .Include(s => s.Reservations)
                                  .ThenInclude(r => r.Tables)
                                  .FirstAsync(s => s.Id == sittingId);

            var m = new ReservationInformationVM()
            {
                StartTime = Helper.GetTimeDropDown(sitiing.Start, sitiing.End, 30),
                Duration = Helper.GetTimeDropDown(),
                ReservationOrigins = new SelectList(await _context.ReservationOrigins.Where(r => r.Id != 4 && r.Id != 5).ToListAsync(), "Id", "Description"),
                RestaurantId = sitiing.RestaurantId,
                RestaurantName = sitiing.Restaurant.Name,
                SittingName = sitiing.Name,
                SittingTypeName = sitiing.SittingType.Description,
                Start = sitiing.Start,
                End = sitiing.End,
                Guest = 1
            };

            return View(m);
        }
        //Post
        [HttpPost]
        public async Task<IActionResult> Confirm(ReservationInformationVM m)
        {

            var sitting = await _context.Sittings.Where(s => s.Id == m.SittingId)
                                                 .Include(s => s.Reservations)
                                                 .FirstAsync();
            if (m.Guest > sitting.Vacancies)//Good way
            {
                ModelState.AddModelError(key: "Guest", errorMessage: $"Not enough vacancies ( Available seats: {sitting.Vacancies})");
            }
            if (!ModelState.IsValid)
            {
                m.StartTime = Helper.GetTimeDropDown(sitting.Start, sitting.End, 30);
                m.Duration = Helper.GetTimeDropDown();
                m.ReservationOrigins = new SelectList(await _context.ReservationOrigins.Where(r => r.Id != 4 && r.Id != 5).ToListAsync(), "Id", "Description");
                m.SittingId = m.SittingId;
                m.RestaurantName = m.RestaurantName;

                return View("ReservationInformation", m);
            }

            if (sitting.Vacancies < m.Guest)
            {
                return PartialView("_NoVacancy", m);
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
                ReservationOriginId = int.Parse(m.SelectedOrigin),
                ReservationStatusId = 1,
                ReservationCode = Guid.NewGuid().ToString("N")
            };
            sitting.Reservations.Add(r);

            await _context.SaveChangesAsync();



            return RedirectToAction("Confirmation", new { reservationId = r.Id });
        }
        public async Task<IActionResult> Confirmation(int reservationId)
        {
            var reservation = await _context.Reservations.Where(r => r.Id == reservationId)
                .Include(r => r.Customer)
                .Include(r => r.Tables)
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

            };
            return View(m);
        }

        public async Task<IActionResult> ReservationEdit(int reservationId)
        {

            var customerId = await _context.Reservations.Where(r => r.Id == reservationId).Select(r => r.CustomerId).FirstAsync();
            var customer = await _context.Customers.Where(c => c.Id == customerId).FirstAsync();
            var reservation = await _context.Reservations.Where(r => r.Id == reservationId)
                                                 .Include(r => r.ReservationStatus)
                                                 .Include(r => r.Customer)
                                                 .Include(r => r.Sitting)
                                                 .FirstAsync();
            var sittingId = await _context.Reservations.Where(r => r.Id == reservationId).Select(r => r.SittingId).FirstAsync();
            var tables = await _context.Tables.Where(t => !t.Reservations.Any(r => r.SittingId == sittingId)).ToListAsync();
            var sitting = await _context.Sittings
                                   .Include(s => s.Restaurant)
                                   .Include(s => s.SittingType)
                                   .Include(s => s.Reservations.Where(r => r.Id == reservationId))
                                   .ThenInclude(r => r.Tables)
                                   .FirstAsync(s => s.Id == sittingId);

            var m = new ReservationEditVM()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Reservation = reservation,
                StartTime = Helper.GetTimeDropDown(sitting.Start, sitting.End, 30),
                Duration = Helper.GetTimeDropDown(),
                ReservationStatuses = new SelectList(await _context.ReservationStatuses.Where(rs => rs.Id >= reservation.ReservationStatusId).ToListAsync(), "Id", "Description"),
                Areas = new SelectList(await _context.Areas.ToListAsync(), "Id", "Name"),
                StatusDescription = await _context.ReservationStatuses.Where(r => r.Id == reservation.ReservationStatusId).Select(r => r.Description).FirstAsync(),
                Tables = tables,
                RestaurantName = sitting.Restaurant.Name
            };


            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> ReservationEdit(ReservationEditVM m)
        {
            var reservation = await _context.Reservations.Where(r => r.Id == m.Reservation.Id)
                                                         .Include(r => r.Sitting)
                                                         .Include(r => r.ReservationOrigin)
                                                         .Include(r => r.ReservationStatus)
                                                         .Include(r => r.Customer)
                                                         .Include(r => r.Tables)
                                                         .FirstAsync();
            //ToDO ADD validation 
            //if (m.Reservation.Guest > reservation.Sitting.Vacancies)
            //{
            //    ModelState.AddModelError(key: "Reservation.Guest", errorMessage: $"Available seats: {reservation.Sitting.Vacancies}");
            //}
            //if (!ModelState.IsValid)
            //{
            //    m.StatusDescription = await _context.ReservationStatuses.Where(r => r.Id == reservation.ReservationStatusId).Select(r => r.Description).FirstAsync();
            //    m.RestaurantName = m.RestaurantName;
            //    m.Reservation = m.Reservation;


            //    return View("ReservationEdit", m);
            //}


                reservation.Guest = m.Reservation.Guest;
                reservation.ReservationStatusId = m.Reservation.ReservationStatusId;
                reservation.Note = m.Reservation.Note;
                if (m.Reservation.ReservationStatusId == 4 || m.Reservation.ReservationStatusId == 5)
                {
                    reservation.Tables.Clear();
                }
                else
                {

                    reservation.Tables.AddRange(m.Tables.Where(t => t.IsBooked == true));
                }
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));




            }








        }
    }
