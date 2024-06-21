using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Models;
using System.Data;
using System.Diagnostics;
using System.Xml;

namespace Strava2ExcelWebApiBackend.Data
{
    public class StravaDbContext : DbContext
    {
        public StravaDbContext(DbContextOptions<StravaDbContext> options) : base(options)
        {

        }

        //public DbSet<Activity> Activities { get; set; }
        public DbSet<Athlete> Athletes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration for Activities entity if needed
            modelBuilder.Entity<Models.Activity>(entity =>
            {
                entity.HasKey(e => e.id);
                // Add any additional configuration here
            });
        }

    }
}
