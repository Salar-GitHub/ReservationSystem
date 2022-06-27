using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RRS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Sitting> Sittings { get; set; }
        public DbSet<SittingType> SittingTypes { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationOrigin> ReservationOrigins { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        // TODO: Ask Peter about Person(Abstract class) Is it needed?

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Employee>().ToTable("Employee");

            builder.Entity<Customer>().Property("UserId").HasColumnType("nvarchar(450)").IsRequired(false);
            builder.Entity<Employee>().Property("UserId").HasColumnType("nvarchar(450)").IsRequired(false);

            base.OnModelCreating(builder);



            //
            //builder.Entity<Restaurant>()
            //    .HasOne(r => r.Company)
            //    .WithMany(c => c.Restaurants)
            //    .OnDelete(DeleteBehavior.Restrict);           
            //builder.Entity<Area>()
            //    .HasOne(a=>a.Restaurant)
            //    .WithMany(r => r.Areas)
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Reservation>()
            //    .HasOne(r => r.Customer)
            //    .WithMany(c => c.Reservations)
            //    .OnDelete(DeleteBehavior.Restrict); 
            //builder.Entity<Customer>()
            //    .HasOne(c => c.Restaurant)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Employee>()
            //    .HasOne(e => e.Restaurant)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Sitting>()
            //    .HasOne(s => s.Restaurant)                
            //    .WithMany(r => r.Sittings)
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Sitting>()
            //    .HasOne(s => s.SittingType)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Table>()
            //    .HasOne(t => t.Area)
            //    .WithMany(a => a.Tables)
            //    .OnDelete(DeleteBehavior.Restrict);





            //TODO Ask Peter about this
            //builder.Entity<Reservation>()
            //    .HasMany(r => r.Tables)
            //    .WithMany(t => t.Reservations)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "ReservationTable",
            //        j => j
            //        .HasOne<Table>()
            //        .WithMany()
            //        .HasForeignKey("TableId")
            //        .HasConstraintName("FK_ReservationTable_Table")
            //        .OnDelete(DeleteBehavior.Restrict),
            //        j => j
            //        .HasOne<Reservation>()
            //        .WithMany()
            //        .HasForeignKey("ReservationId")
            //        .HasConstraintName("FK_ReservationTable_Reservation")
            //        .OnDelete(DeleteBehavior.Restrict)
            //        );




        }
    }
}