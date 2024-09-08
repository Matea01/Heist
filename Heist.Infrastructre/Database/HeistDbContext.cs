using Heist.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heist.Infrastructure.Database
{
    public class HeistDbContext : DbContext
    {
        public DbSet<Member> Member { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<MemberSkill> MemberSkills { get; set; }
        public DbSet<HeistEntity> HeistEntity { get; set; }
        public DbSet<HeistSkillRequirement> HeistSkillRequirements { get; set; }

        public HeistDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure unique constraint on Skill name
            modelBuilder.Entity<Skill>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Configure the many-to-many relationship between Member and Skill
            modelBuilder.Entity<MemberSkill>()
                .HasKey(ms => new { ms.MemberId, ms.SkillId });

            modelBuilder.Entity<MemberSkill>()
                .HasOne(ms => ms.Member)
                .WithMany(m => m.MemberSkills)
                .HasForeignKey(ms => ms.MemberId);

            modelBuilder.Entity<MemberSkill>()
                .HasOne(ms => ms.Skill)
                .WithMany()
                .HasForeignKey(ms => ms.SkillId);

            
            modelBuilder.Entity<HeistEntity>()
                .HasIndex(h => h.Name)
                .IsUnique();

            
            modelBuilder.Entity<HeistSkillRequirement>()
                .HasKey(hsr => new { hsr.HeistId, hsr.SkillId });

            modelBuilder.Entity<HeistSkillRequirement>()
                .HasOne(hsr => hsr.Heist)
                .WithMany(h => h.SkillRequirements)
                .HasForeignKey(hsr => hsr.HeistId);

            modelBuilder.Entity<HeistSkillRequirement>()
                .HasOne(hsr => hsr.Skill)
                .WithMany()
                .HasForeignKey(hsr => hsr.SkillId);

            
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            
            modelBuilder.Entity<HeistEntity>()
                .HasIndex(h => h.Name)
                .IsUnique();
        }

    }
}
