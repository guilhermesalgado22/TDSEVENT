using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanejaiBack.Models;
using System.Diagnostics;
using System.Xml;

namespace PlanejaiBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ActivityModel>? Activities { get; set; }
        public DbSet<EventModel>? Events { get; set; }
        public DbSet<EventsGuests>? EventsGuests { get; set; }
        public DbSet<ScheduleModel>? Schedules { get; set; }
        public DbSet<GuestModel>? Guests { get; set; }
        public DbSet<UserModel>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityModel>().ToTable("Activities").HasKey(e => e.ActivityId);
            modelBuilder.Entity<EventModel>().ToTable("Events").HasKey(e => e.EventId);
            modelBuilder.Entity<EventsGuests>().HasKey(eg => new { eg.EventId, eg.GuestId });
            modelBuilder.Entity<ScheduleModel>().ToTable("Schedules").HasKey(s => s.ScheduleId);
            modelBuilder.Entity<GuestModel>().ToTable("Guests").HasKey(g => g.GuestId);
            modelBuilder.Entity<UserModel>().ToTable("Users").HasKey(u => u.UserId);

            modelBuilder.Entity<ActivityModel>()
                .HasOne<ScheduleModel>(u => u.Schedule)
                .WithMany(u => u.Activities)
                .HasForeignKey(u => u.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScheduleModel>()
                .HasOne<EventModel>(u => u.Event)
                .WithOne(u => u.Schedule)
                .HasForeignKey<ScheduleModel>(u => u.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventModel>()
                .HasOne<UserModel>(u => u.Organizer)
                .WithMany(u => u.Events)
                .HasForeignKey(u => u.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventsGuests>()
                .HasOne<EventModel>(eg => eg.Event)
                .WithMany(e => e.EventsGuests)
                .HasForeignKey(eg => eg.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventsGuests>()
                .HasOne<GuestModel>(eg => eg.Guest)
                .WithMany(e => e.EventsGuests)
                .HasForeignKey(eg => eg.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            #region Date Config.
            modelBuilder.Entity<EventModel>()
                .Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<EventModel>()
                .Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<EventModel>()
                .Property(e => e.StartsAt)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<EventModel>()
                .Property(e => e.EndsAt)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<ActivityModel>()
                .Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<ActivityModel>()
                .Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<ActivityModel>()
                .Property(e => e.StartsAt)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<ActivityModel>()
                .Property(e => e.EndsAt)
                .HasColumnType("timestamp without time zone");
            #endregion
        }
    }
}
