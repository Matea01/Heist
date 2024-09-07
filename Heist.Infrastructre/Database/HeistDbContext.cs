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
        public DbSet<Member> Member { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .OwnsMany(m => m.Skills, s =>
                {
                    s.WithOwner().HasForeignKey("MemberId"); // Shadow property* for the foreign key
                    s.Property(skill => skill.Name).HasColumnName("SkillName");
                    s.Property(skill => skill.Level).HasColumnName("SkillLevel");
                });
        }
        public HeistDbContext(DbContextOptions options) : base(options)
        { }
    }
}
