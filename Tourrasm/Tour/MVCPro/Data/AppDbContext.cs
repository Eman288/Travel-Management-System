using Microsoft.EntityFrameworkCore;
using MVCPro.Models;

namespace MVCPro.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here   
            modelBuilder.Entity<TripAtt>()
                 .HasKey("TripId", "TouristAttractionId", "Order");
            modelBuilder.Entity<UserTrip>()
                 .HasKey("TripId", "UserId");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> trips { get; set; }
        public DbSet<UserTrip> usertrips { get; set; }
        public DbSet<TouristAttraction> tourists { get; set; }
        public DbSet<Bus> buses { get; set; }
        public DbSet<Staff> staff { get; set; }
        public DbSet<TripAtt> tripatt { get; set; }


    }
}
