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




    }
}
