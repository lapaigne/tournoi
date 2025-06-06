using Microsoft.EntityFrameworkCore;
using Tournoi.Models;

namespace Tournoi.DB
{
    public partial class AppDBContext : DbContext
    {
        public DbSet<PersonModel> People => Set<PersonModel>();
        public DbSet<PlayerModel> Players => Set<PlayerModel>();
        public DbSet<EQTeamModel> EQTeams => Set<EQTeamModel>();
        public DbSet<PairModel> Pairs => Set<PairModel>();

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public AppDBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}