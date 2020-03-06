using deft_pay_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace deft_pay_backend.DBContexts
{
    public class MariaDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<TransactionToken> TransactionTokens { get; set; }
        public virtual DbSet<IdentityUserLogin<Guid>> UserLogins { get; set; }
        
        public MariaDbContext(DbContextOptions<MariaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(bc => new { bc.UserName })
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(bc => new { bc.BVN })
                .IsUnique();

            modelBuilder.Entity<IdentityUserLogin<Guid>>()
                .ToTable("UserLogins")
                .HasKey(x => new { x.UserId, x.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<Guid>>()
                .ToTable("UserRoles")
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<TransactionToken>()
                .HasIndex(bc => new { bc.OTP })
                .IsUnique();

            modelBuilder.Entity<TransactionToken>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.TransactionTokens)
                .HasForeignKey(bc => bc.UserId);

        }
    }
}
