using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RRS.Models.Booking
{
    public class InquiryVM
    {
        [DataType(DataType.Date)]
        public DateTime SelectedDate { get; set; }
        public SelectList SittingTypes { get; set; }       
        public string Description { get; set; }
    }
}
