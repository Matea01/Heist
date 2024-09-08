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

        public async Task<Skill> GetSkillByNameAsync(string skillName)
        {
            return await _dbContext.Skills
                .FirstAsync(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Skill> AddSkillAsync(Skill skill)
        {
            _dbContext.Skills.Add(skill);
            await _dbContext.SaveChangesAsync();
            return skill;
        }
    }
}
