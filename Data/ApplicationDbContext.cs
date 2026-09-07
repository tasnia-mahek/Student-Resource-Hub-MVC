using Microsoft.EntityFrameworkCore;
using student_resource_hub.Models;

namespace student_resource_hub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);
                entity.Property(user => user.FullName).HasMaxLength(120).IsRequired();
                entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
                entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
                entity.Property(user => user.Role).HasMaxLength(20).IsRequired();
                entity.HasIndex(user => user.Email).IsUnique();
            });
        }
    }
}