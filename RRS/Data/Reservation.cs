  namespace RRS.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public DateTime EndTime { get => StartTime.AddMinutes(Duration); }
        public string? Note { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int Guest { get; set; }

        public int SittingId { get; set; }
        public Sitting Sitting { get; set; }
        
        public int ReservationOriginId { get; set; }
        public ReservationOrigin ReservationOrigin { get; set; }
        
        public int ReservationStatusId { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public string ReservationCode { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();


    }
}
