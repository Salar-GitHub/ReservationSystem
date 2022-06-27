using Microsoft.AspNetCore.Mvc.Rendering;
using RRS.Data;

namespace RRS.Areas.Employee.Models
{
    public class FilterVm
    {
        public SelectList ReservationStatuses { get; set; }
        public string StatusDescription { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
