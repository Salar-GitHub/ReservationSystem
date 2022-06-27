using System.ComponentModel.DataAnnotations;

namespace RRS.Areas.Manager.Models
{
    public class DateTimeValidator : ValidationAttribute
    {
        public DateTimeValidator()
        {

        }

        public override bool IsValid(object value)
        {
            var datetime = (DateTime)value;
            if (datetime >= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
