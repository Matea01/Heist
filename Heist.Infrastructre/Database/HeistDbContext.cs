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
            // Configuring unique constraints
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Storing enums as strings in the database
            modelBuilder.Entity<Member>()
                .Property(m => m.Sex)
                .HasConversion<string>(); // Store Sex enum as string

            modelBuilder.Entity<Member>()
                .Property(m => m.Status)
                .HasConversion<string>(); // Store Status enum as string

            // Optional: Set up cascading behavior for MainSkill
            modelBuilder.Entity<Member>()
                .HasOne(m => m.MainSkill)
                .WithMany()
                .HasForeignKey(m => m.MainSkillId)
                .OnDelete(DeleteBehavior.SetNull); // Optional relationship, so if skill is deleted, MainSkill is set to null
        }
        public HeistDbContext(DbContextOptions options) : base(options)
        { }
    }
}
