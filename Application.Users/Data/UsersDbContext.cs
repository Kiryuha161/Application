using Application.Shared.Database;
using Application.Users.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Data
{
    public class UsersDbContext : BaseDbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options, "users") { }
        
        public DbSet<UserProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("user_profiles", "users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();

                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
            });
                
        }
    }
}
