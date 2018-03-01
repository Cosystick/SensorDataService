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
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sensors.db");
        }
    }
}