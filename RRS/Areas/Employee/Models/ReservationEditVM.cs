using Microsoft.AspNetCore.Mvc.Rendering;
using RRS.Data;

namespace RRS.Areas.Employee.Models
{
    public class ReservationEditVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Reservation Reservation { get; set; }

        public SelectList ReservationStatuses { get; set; }        
        public string StatusDescription { get; set; }

        public SelectList Areas { get; set; }
        public string SelectedArea { get; set; }

        public List<Table> Tables { get; set; }

        public List<SelectListItem> Duration { get; set; }
        public string SelectedDuration { get; set; }

        public List<SelectListItem> StartTime { get; set; }
        public string SelectedTime { get; set; }

        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }

        public int SittingId { get; set; }
    }
}