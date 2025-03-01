using Microsoft.EntityFrameworkCore;
using FitmetricModel = Strava2ExcelWebApiBackend.Models;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Text.Json;
using Strava2ExcelWebApiBackend.Models;

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
        public DbSet<ActivityDetails> ActivityDetails { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<User> Athletes { get; set; }
        public object Users { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SplitMetric>().HasNoKey();
            //modelBuilder.Entity<SegmentEffort>().HasNoKey();
            //modelBuilder.Entity<Segment>().HasNoKey();
            //modelBuilder.Entity<Achievement>().HasNoKey();


            modelBuilder.Entity<FitmetricModel.Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Map>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            // One-to-One relationship between Activity and ActivityDetails
            //modelBuilder.Entity<FitmetricModel.Activity>()
            //    .HasOne(a => a.ActivityDetails)  // Activity has one ActivityDetails
            //    .WithOne(ad => ad.Activity)      // ActivityDetails has one Activity
            //    .HasForeignKey<ActivityDetails>(ad => ad.ActivityId);

        }
    }
}
