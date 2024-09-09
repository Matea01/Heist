using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly HeistDbContext _dbContext;

        public SkillRepository(HeistDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Skill?> GetSkillByNameAsync(string skillName)
        {
            return await _dbContext.Skills.Where(s => s.Name.ToLower() == skillName.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<Skill> AddSkillAsync(Skill skill)
        {
            try
            {
                _dbContext.Skills.Add(skill);
                await _dbContext.SaveChangesAsync();
                return skill;
            }
            catch (DbUpdateException ex)
            {
                // Log or inspect the exception details here
                Console.WriteLine(ex.Message);
                throw; // Re-throw the exception to keep the stack trace
            }
        }
        public async Task<MemberSkill?> GetMemberSkillAsync(int memberId, int skillId)
        {
            return await _dbContext.MemberSkills.Where(s => s.MemberId == memberId && s.SkillId == skillId)
                .FirstOrDefaultAsync();
        }

    }
}
