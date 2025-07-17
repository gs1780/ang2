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
                entity.Property(u => u.Id).HasColumnName("id");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
                entity.Property(u => u.Name).HasColumnName("name").HasMaxLength(255);
                entity.Property(u => u.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.CreatedBy).HasColumnName("created_by");
                entity.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("id");
                entity.HasIndex(r => r.Name).IsUnique();
                entity.Property(r => r.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(r => r.Description).HasColumnName("description");
                entity.Property(r => r.RoleType).HasColumnName("role_type").HasMaxLength(20).IsRequired();
                entity.Property(r => r.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(r => r.CreatedBy).HasColumnName("created_by");
                entity.Property(r => r.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
                entity.Property(r => r.UpdatedBy).HasColumnName("updated_by");
                entity.HasCheckConstraint("chk_role_type", "role_type IN ('global','scoped')");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.Property(ur => ur.UserId).HasColumnName("user_id");
                entity.Property(ur => ur.RoleId).HasColumnName("role_id");
                entity.Property(ur => ur.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(ur => ur.CreatedBy).HasColumnName("created_by");
                entity.Property(ur => ur.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
                entity.Property(ur => ur.UpdatedBy).HasColumnName("updated_by");
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
