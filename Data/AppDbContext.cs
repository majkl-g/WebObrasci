using Microsoft.EntityFrameworkCore;
using WebObrasci1.Models;
using Microsoft.Extensions.Configuration;

namespace WebObrasci1.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public AppDbContext()
        {
        }

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (_configuration != null)
            {
                options.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
            }
            else
            {
                // Fallback for design-time tools
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                options.UseNpgsql(config.GetConnectionString("ConnectionString"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite primary key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // UserRole -> User relationship
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            // UserRole -> Role relationship
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
    }
}
