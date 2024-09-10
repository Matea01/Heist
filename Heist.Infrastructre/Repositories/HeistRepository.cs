using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Heist.Infrastructure.Repositories
{
    public class HeistRepository : IHeistRepository
    {
        private readonly HeistDbContext _dbContext;

        public HeistRepository(HeistDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> HeistExistsAsync(string name)
        {
            return await _dbContext.HeistEntity.AnyAsync(h => h.Name == name);
        }

        public async Task AddHeistAsync(HeistEntity heist)
        {
            _dbContext.HeistEntity.Add(heist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<HeistEntity> GetHeistByIdAsync(int heistId)
        {
            return await _dbContext.HeistEntity
            .Include(h => h.SkillRequirements)
            .ThenInclude(hsr => hsr.Skill)
            .FirstAsync(h => h.Id == heistId);
        }

        public async Task<HeistEntity?> GetHeistByNameAsync(string? name)
        {
            return await _dbContext.HeistEntity
            .FirstOrDefaultAsync(h => h.Name.ToLower() == name.ToLower());
        }

        public async Task UpdateHeistAsync(HeistEntity heist)
        {
            _dbContext.HeistEntity.Update(heist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<HeistSkillRequirement?> GetHeistSkillRequirementAsync(int heistId, int skillId)
        {
            return await _dbContext.HeistSkillRequirements
                .FirstOrDefaultAsync(hsr => hsr.HeistId == heistId && hsr.SkillId == skillId);
        }
        public async Task UpdateHeistSkillsAsync(int heistId, List<HeistSkillRequirement> updatedSkills)
        {
            var heist = await _dbContext.HeistEntity
                .Include(h => h.SkillRequirements)
                .FirstOrDefaultAsync(h => h.Id == heistId);

            if (heist != null)
            {
                _dbContext.HeistSkillRequirements.RemoveRange(heist.SkillRequirements);
                heist.SkillRequirements = updatedSkills;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
