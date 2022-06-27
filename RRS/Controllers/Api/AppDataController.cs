using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using System.Linq;
using RRS.Models;

namespace RRS.Controllers.Api
{
    [Route("api/v1/AppData")]
    [ApiController]
    public class AppDataController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _customerManager;
        private readonly UserManager<IdentityUser> _sittingManager;
        private readonly ApplicationDbContext _context;

        public AppDataController(UserManager<IdentityUser> customerManager, ApplicationDbContext context, UserManager<IdentityUser> sittingManager)
        {
            _customerManager = customerManager;
            _context = context;
            _sittingManager = sittingManager;
        }

        [HttpGet("me")]
        public async Task<CustomerData> GetCurrentUser()
        {
            //string[] Roles = await _customerManager.GetRolesAsync(User);
            IdentityUser? customer = await _customerManager.GetUserAsync(User);
            
            if (customer == null)
            {
                return new CustomerData();
            }
            return new CustomerData
            {
                Authorized = true,
                UserName = customer.UserName,
                Roles = (await _customerManager.GetRolesAsync(customer)).ToArray(),
            };
        }


        [Authorize]
        [HttpGet("reservations")]
        public async Task<IActionResult> GetUserReservations()
        {
            var userId = _customerManager.GetUserId(User);
            if (userId == null)
            {
                return BadRequest();
            }
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Sitting)
                .Where(r => r.Customer.UserId == userId)
                .Select(r => new AppData()
                {
                    SittingId = r.SittingId,
                    SittingName = r.Sitting.Name,
                    SittingStart = r.Sitting.Start.ToString("yyyy-MM-dd hh:mm tt"),
                    SittingEnd = r.Sitting.End.ToString("yyyy-MM-dd hh:mm tt"),
                    RestaurantId = r.Sitting.RestaurantId,
                    RestaurantName = r.Sitting.Restaurant.Name,
                    ReservationId = r.Id,
                    ReservationStart = r.StartTime.ToString("yyyy-MM-dd hh:mm tt"),
                    ReservationEnd = r.EndTime.ToString("yyyy-MM-dd hh:mm tt"),
                    ReservationDuration = r.Duration,
                    ReservationNote = r.Note,
                    ReservationCustomerId = r.CustomerId,
                    ReservationGuest = r.Guest,
                    ReservationSittingId = r.SittingId,
                    ReservationOriginId = r.ReservationOriginId,
                    ReservationStatusId = r.ReservationStatusId,
                    ReservationCode = r.ReservationCode,
                })
                .ToListAsync();
            return Ok(reservations);

        }

        [Authorize]
        [HttpGet("all-reservations")]
        public async Task<IActionResult> GetAllReservations()
        {
            var userId = _customerManager.GetUserId(User);
            if (userId == null)
            {
                return BadRequest();
            }
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Sitting)
                .Select(r => new AppData()
                {
                    SittingId = r.SittingId,
                    SittingName = r.Sitting.Name,
                    SittingStart = r.Sitting.Start.ToString("yyyy-MM-dd hh:mm tt"),
                    SittingEnd = r.Sitting.End.ToString("yyyy-MM-dd hh:mm tt"),
                    RestaurantId = r.Sitting.RestaurantId,
                    RestaurantName = r.Sitting.Restaurant.Name,
                    ReservationId = r.Id,
                    ReservationStart = r.StartTime.ToString("yyyy-MM-dd hh:mm tt"),
                    ReservationEnd = r.EndTime.ToString("yyyy-MM-dd hh:mm tt"),
                    ReservationDuration = r.Duration,
                    ReservationNote = r.Note,
                    ReservationCustomerId = r.CustomerId,
                    ReservationGuest = r.Guest,
                    ReservationSittingId = r.SittingId,
                    ReservationOriginId = r.ReservationOriginId,
                    ReservationStatusId = r.ReservationStatusId,
                    ReservationCode = r.ReservationCode,
                    TableId = r.Tables.Select(t => t.Id).ToArray(),
                    TableName = r.Tables.Select(t => t.Name).ToArray(),

                })
                .ToListAsync();
            return Ok(reservations);
        }
        

    }
}
