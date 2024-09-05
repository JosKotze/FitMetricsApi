using Microsoft.EntityFrameworkCore;
using FitmetricModel = Strava2ExcelWebApiBackend.Models;
using System.Data;
using System.Diagnostics;
using System.Xml;

// command for db context update:
// dotnet ef migrations add InitialCreate -o Data/Migrations
// dotnet ef database update

namespace Strava2ExcelWebApiBackend.Data
{
    public class StravaDbContext : DbContext
    {
        public StravaDbContext(DbContextOptions<StravaDbContext> options) : base(options)
        {

        }

        public DbSet<FitmetricModel.Activity> Activities { get; set; }
        public DbSet<FitmetricModel.User> Athletes { get; set; }
        public object Users { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration for Activities entity if needed
            modelBuilder.Entity<Models.Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Add any additional configuration here
            });
        }

    }
}
