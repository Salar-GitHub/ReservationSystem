namespace RRS.Data
{
    public class Sitting
    {

        public Sitting() {}

        public Sitting(int restaurantId, int sittingTypeId, string name, DateTime start, DateTime end,int capacity, bool isPrivate, bool isClosed)
        {
            RestaurantId = restaurantId;
            SittingTypeId = sittingTypeId;
            Name = name;
            Start = start;
            End = end;
            Capacity = capacity;
            IsPrivate = isPrivate;
            IsClosed = isClosed; 
        }
        public int Id { get; set; }
        public string Name { get; set; }

       
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int Capacity { get; set; }
        public int Pax { get => Reservations.Sum(r => r.Guest); }
        public int Vacancies { get => Capacity - Pax; }

        public bool IsPrivate { get; set; }
        public bool IsClosed { get; set; }
        

        public Restaurant Restaurant { get; set; }
        public int RestaurantId{ get; set; }

        public SittingType SittingType { get; set; }
        public int SittingTypeId { get; set; }       

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
