namespace RRS.Data
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ABN { get; set; }

        public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
    }
}
