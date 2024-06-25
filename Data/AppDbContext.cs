using Microsoft.EntityFrameworkCore;
using VacinasApi.Postos;
using VacinasApi.Vacinas;

namespace VacinasApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Posto> Postos { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Db.sqlite");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
