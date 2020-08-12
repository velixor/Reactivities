using Domain;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Value> Values { get; [UsedImplicitly] set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Value>().HasData(new Value {Id = 1, Name = "Value1"},
                new Value {Id = 2, Name = "Value2"},
                new Value {Id = 3, Name = "Value3"});

            modelBuilder.Entity<UserActivity>()
                .HasKey(ua => new {ua.AppUserId, ua.ActivityId});

            modelBuilder.Entity<UserActivity>()
                .HasOne(ua => ua.AppUser)
                .WithMany(au => au.UserActivities)
                .HasForeignKey(ua => ua.AppUserId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(ua => ua.Activity)
                .WithMany(a => a.UserActivities)
                .HasForeignKey(ua => ua.ActivityId);
        }
    }
}