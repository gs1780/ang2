using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("auth");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
                entity.Property(u => u.Name).HasMaxLength(255);
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.Name).IsUnique();
                entity.Property(r => r.Name).HasMaxLength(50).IsRequired();
                entity.Property(r => r.RoleType).HasMaxLength(20).IsRequired();
                entity.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(r => r.UpdatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasCheckConstraint("chk_role_type", "role_type IN ('global','scoped')");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.Property(ur => ur.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(ur => ur.UpdatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
