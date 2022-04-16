using AutoRapide.Utilisateurs.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoRapide.Utilisateurs.API.Data
{
    public class UsagerContext : DbContext
    {
        public DbSet<Usager>? Usagers { get; set; }

        public UsagerContext(DbContextOptions<UsagerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usager>().ToTable("Usager");
        }
    }
}