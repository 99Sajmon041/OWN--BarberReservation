using BarberReservation.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BarberReservation.Infrastructure.Database;

public sealed class BarberDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<HairdresserService> HairdresserServices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<HairdresserWorkingHours> HairdresserWorkingHours { get; set; }
    public DbSet<HairdresserTimeOff> HairdresserTimeOffs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Service>()
            .HasIndex(s => s.Name)
            .IsUnique();

        builder.Entity<Reservation>()
            .HasIndex(r => new { r.CustomerEmail, r.CustomerPhone });

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.WorkingHours)
            .WithOne(w => w.Hairdresser)
            .HasForeignKey(w => w.HairdresserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.TimeOffs)
            .WithOne(t => t.Hairdresser)
            .HasForeignKey(t => t.HairdresserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.HairdresserServices)
            .WithOne(hs => hs.Hairdresser)
            .HasForeignKey(hs => hs.HairdresserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Service>()
            .HasMany(s => s.HairdresserServices)
            .WithOne(hs => hs.Service)
            .HasForeignKey(hs => hs.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HairdresserService>()
            .HasMany(hs => hs.Reservations)
            .WithOne(r => r.HairdresserService)
            .HasForeignKey(r => r.HairdresserServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.HairdresserReservations)
            .WithOne(r => r.Hairdresser)
            .HasForeignKey(r => r.HairdresserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.CustomerReservations)
            .WithOne(r => r.Customer)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<HairdresserService>()
            .HasIndex(x => new { x.HairdresserId, x.ServiceId })
            .IsUnique()
            .HasFilter("[IsActive] = 1");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HaveColumnType("decimal(18,2)");
    }
}
