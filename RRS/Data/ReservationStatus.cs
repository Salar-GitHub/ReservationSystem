namespace RRS.Data
{
    public class ReservationStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Reservation> Reservations { get; set; }

    }
}
