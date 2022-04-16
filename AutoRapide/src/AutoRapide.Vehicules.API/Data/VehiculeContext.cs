using AutoRapide.Vehicules.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoRapide.Vehicules.API.Data;

public class VehiculeContext : DbContext
{
    public DbSet<Vehicule> Vehicules { get; set; }

    public VehiculeContext(DbContextOptions<VehiculeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicule>().ToTable("Vehicule");
    }
}