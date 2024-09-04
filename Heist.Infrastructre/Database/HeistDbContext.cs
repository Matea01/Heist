using Heist.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Infrastructure.Database
{
    public class HeistDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("DefaultConnection",
        //        options => options.MigrationsAssembly("Heist.Infrastructure"));
        //}

        public DbSet<Member> Member { get; set; }
        public DbSet<Skill> Skill { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between Member and Skill
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Skills)
                .WithOne(s => s.HeistMember)
                .HasForeignKey(s => s.HeistMemberId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Additional configuration if needed
        }
        public HeistDbContext(DbContextOptions options) : base(options)
        { }
    }
}
