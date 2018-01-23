using Microsoft.EntityFrameworkCore;

namespace SensorService.API.Models
{
    public class SensorContext : DbContext
    {
        public SensorContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sensors.db");
        }
    }
}