using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using VacinasApi.Models;

namespace VacinasApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Posto> Postos { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posto>()
                .HasIndex(p => p.Nome)
            .IsUnique();

            modelBuilder.Entity<Vacina>()
                .HasIndex(v => v.Lote)
            .IsUnique();

            modelBuilder.Entity<Vacina>()
                .HasOne(v => v.Posto)
                .WithMany(p => p.Vacinas)
                .HasForeignKey(v => v.PostoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
