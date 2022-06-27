using System.ComponentModel.DataAnnotations.Schema;

namespace RRS.Data
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SeatNumber { get; set; }
        [NotMapped]
        public bool IsBooked { get; set; }
        public Area Area { get; set; }
        public  int  AreaId { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

