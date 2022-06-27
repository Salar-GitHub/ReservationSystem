using Microsoft.EntityFrameworkCore;
using RRS.Data;

namespace RRS.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Customer> FindOrCreateCustomerAsync(string firstname, string lastname, string email, string phoneNumber,int restaurantId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    RestaurantId = restaurantId
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            return customer;
        }
    }
}
