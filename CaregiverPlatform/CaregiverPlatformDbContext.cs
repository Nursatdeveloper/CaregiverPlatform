using CaregiverPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CaregiverPlatform {
    public class CaregiverPlatformDbContext : DbContext{
        public CaregiverPlatformDbContext(DbContextOptions<CaregiverPlatformDbContext> options) : base(options) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> TbUsers { get; set; }
        public DbSet<Caregiver> TbCaregivers { get; set; }
        public DbSet<Member> TbMembers { get; set; }
        public DbSet<Job> TbJobs { get; set; }
        public DbSet<JobApplication> TbJobApplications { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().ToTable(t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Caregiver>().ToTable(t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Member>().ToTable(t => t.ExcludeFromMigrations());
            modelBuilder.Entity<Job>().ToTable(t => t.ExcludeFromMigrations());
            modelBuilder.Entity<JobApplication>().ToTable(t => t.ExcludeFromMigrations());
        }
    }
}
