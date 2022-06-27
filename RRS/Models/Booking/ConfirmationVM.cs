

namespace RRS.Models.Booking
{
    public class ConfirmationVM
    {
        public int ReservationId { get; set; }
        public string ReservationCode { get; set; }
        public string RestaurantName { get; set; }
        public string SittingName { get; set; }
        public string SittingTypeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public int Guest { get; set; }
        public string? Note { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<RRS.Data.Table> Tables { get; set; }
    }
}
