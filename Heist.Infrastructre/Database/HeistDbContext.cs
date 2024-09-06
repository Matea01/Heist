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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the owned type 'Skills' as a collection
            modelBuilder.Entity<Member>()
                .OwnsMany(m => m.Skills, s =>
                {
                    s.WithOwner().HasForeignKey("MemberId"); // Shadow property for the foreign key
                    s.Property(skill => skill.Name).HasColumnName("SkillName");
                    s.Property(skill => skill.Level).HasColumnName("SkillLevel");
                });

            // Configuring the owned type 'MainSkill' as a single entity
            modelBuilder.Entity<Member>()
                .OwnsOne(m => m.MainSkill, s =>
                {
                    s.Property(skill => skill.Name).HasColumnName("MainSkillName");
                    s.Property(skill => skill.Level).HasColumnName("MainSkillLevel");
                });

            // Optionally, you may want to configure any additional constraints or settings
            // Example: if 'MainSkillId' is nullable, ensure it is correctly handled in your model
        }

        public HeistDbContext(DbContextOptions options) : base(options)
        { }
    }
}
