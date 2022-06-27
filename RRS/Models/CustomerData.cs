using RRS.Data;

namespace RRS.Models
{
    public class CustomerData
    {
       public bool Authorized { get; set; }
       public string? UserName { get; set; }
       public string[] Roles { get; set; }

    }
}
