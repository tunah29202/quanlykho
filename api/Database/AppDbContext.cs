using Microsoft.EntityFrameworkCore;
using Database.Entities;
namespace Database
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }   
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Resource>? Resources { get; set; }
        public virtual DbSet<LogAction>? LogActions { get; set; }
        public virtual DbSet<LogException>? LogExceptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}