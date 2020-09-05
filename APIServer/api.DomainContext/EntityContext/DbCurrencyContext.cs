using api.DomainContext.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using DbContextOptionsBuilder = Microsoft.EntityFrameworkCore.DbContextOptionsBuilder;

namespace api.DomainContext.EntityContext
{
    public class DbCurrencyContext : DbContext
    {
        public DbSet<CurrencyRates> CurrencyRates { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        protected DbCurrencyContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=apiDB;Username=postgres;Password=123456");
        }
    }
}