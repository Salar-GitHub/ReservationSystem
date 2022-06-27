namespace RRS.Models
{
    public class AppData
    {
        //sitting
        public int SittingId { get; set; }
        public string? SittingName { get; set; }
        public string? SittingStart { get; set; }
        public string? SittingEnd { get; set; }
        //Resturant
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }
        //Reservation
        public int ReservationId { get; set; }
        public string? ReservationStart { get; set; }
        public string? ReservationEnd { get; set; }
        public int ReservationDuration { get; set; }
        public string? ReservationNote { get; set; }
        public int ReservationCustomerId { get; set; }
        public int ReservationGuest { get; set; }
        public int ReservationSittingId { get; set; }
        public int ReservationOriginId { get; set; }
        public int ReservationStatusId { get; set; }
        public string ReservationCode { get; set; }
        //table
        public int[]? TableId { get; set; }
        public string[]? TableName { get; set; }
    }
}
