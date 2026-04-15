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

        public DbSet<Student> Students { get; set; }
    }
}
