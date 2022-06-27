using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RRS.Areas.Employee.Models
{
    public class ReservationInformationVM
    {

        //--------
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone,Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }
        [Range(1, 100)]
        public int Guest { get; set; }
        [ValidateNever]
        public string? Note { get; set; } 
        [ValidateNever,Display(Name ="Strat Time")]
        public List<SelectListItem> StartTime { get; set; }
        [Display(Name="Start Time")]
        public string SelectedTime { get; set; }
        [ValidateNever]
        public List<SelectListItem> Duration { get; set; }
        [Display(Name = "Duration")]
        public string SelectedDuration { get; set; }
        [ValidateNever]
        [Display(Name = "Reservation Origin")]
        public SelectList ReservationOrigins { get; set; }
        [Display(Name ="Reservation Origin")]
        public string SelectedOrigin { get; set; }
        //---------
        public int SittingId { get; set; }
        [ValidateNever]
        public string SittingName { get; set; }
        [ValidateNever]
        public string SittingTypeName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [ValidateNever]
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }

    }
}



