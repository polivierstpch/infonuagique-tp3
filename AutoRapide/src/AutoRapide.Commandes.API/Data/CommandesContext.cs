using AutoRapide.Commandes.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoRapide.Commandes.API.Data;

public class CommandeContext : DbContext
{
    private DbSet<Commande> Commandes { get; set; }
    
    public CommandeContext(DbContextOptions<CommandeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commande>().ToTable("Commande");
    }
}
