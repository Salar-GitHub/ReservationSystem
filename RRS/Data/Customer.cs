namespace RRS.Data
{
    public class Customer:Person
    {
       public List<Reservation> Reservations = new List<Reservation>();
    }
}
