using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RRS.Areas.Manager.Models
{
    public class CreateEmployeeVM
    {
        public int RestaurantId { get; set; }
        [Required,Display(Name ="First name")]
        public string FirstName { get; set; }
        [Required,Display(Name ="Last name")]
        public string LastName { get; set; }
        [EmailAddress,Display(Name ="email")]
        public string Email { get; set; }
        [Required,Display(Name =("Phone number"))]
        public string PhoneNumber { get; set; }
        [Required, Display(Name = "Tax file number"), MinLength(8, ErrorMessage = "Tax file number must be 8 or 9 digits long."), MaxLength(9, ErrorMessage = "Tax file number must be 8 or 9 digits long.")]
        public string TaxFileNumber { get; set; }
        [ValidateNever]
        public string? UserId { get; set; }
    }
}
