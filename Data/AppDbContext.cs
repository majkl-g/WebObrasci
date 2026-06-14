using Microsoft.EntityFrameworkCore;
using WebObrasci1.Models;

namespace WebObrasci1.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration? configuration, ILogger<AppDbContext> logger)
            : base(options)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (_configuration != null)
            {
                _logger.LogInformation("Using Npgsql");
                options.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
            }
            else
            {
                _logger.LogInformation("EF Core designtime: using Npgsql");

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
            base.OnModelCreating(modelBuilder);

            // Unique ExternalId
            modelBuilder.Entity<User>()
                .HasIndex(u => u.ExternalId)
                .IsUnique();

            modelBuilder.Entity<Form>()
                .HasMany(x => x.Fields)
                .WithOne(x => x.Form)
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Form>()
                .HasMany(x => x.RequiredApprovals)
                .WithOne()
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FormSubmission>()
                .HasOne(x => x.Form)
                .WithMany()
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FormSubmission>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FormSubmission>()
                .HasMany(x => x.Approvals)
                .WithOne()
                .HasForeignKey(x => x.FormSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FormSubmissionApproval>()
                .HasOne(x => x.ApprovalUser)
                .WithMany()
                .HasForeignKey(x => x.ApprovalUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Form> Forms { get; set; }

        public DbSet<FormField> FormFields { get; set; }

        public DbSet<FormRequiredApprovals> FormRequiredApprovals { get; set; }

        public DbSet<FormSubmission> FormSubmissions { get; set; }

        public DbSet<FormSubmissionApproval> FormSubmissionApproval { get; set; }
    }
}
