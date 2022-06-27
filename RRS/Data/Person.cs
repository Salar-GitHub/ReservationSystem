namespace RRS.Data
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RestaurantId { get; set; } = 1;
        public Restaurant Restaurant { get; set; }
        public string? UserId { get; set; }

    }
}
