namespace RRS.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Sitting> Sittings { get; set; } = new List<Sitting>();

        public List<Area> Areas { get; set; } = new List<Area>();

        public Sitting AddSitting(int sittingTypeId, string name,DateTime start,DateTime end,int capacity, bool isPrivate, bool isClosed)
        {
            var sitting = new Sitting(this.Id, sittingTypeId, name, start, end, capacity, isPrivate, isClosed);
            Sittings.Add(sitting);
            return sitting; 
        }

    }
}
