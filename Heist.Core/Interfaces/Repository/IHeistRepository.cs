using Heist.Core.Entities;

namespace Heist.Core.Interfaces.Repository
{

    public interface IHeistRepository
    {
        Task<bool> HeistExistsAsync(string name);
        Task AddHeistAsync(HeistEntity heist);
        Task<HeistEntity> GetHeistByIdAsync(int id);
        Task<HeistEntity?> GetHeistByNameAsync(string name);
        Task UpdateHeistAsync(HeistEntity heist);
        Task UpdateHeistSkillsAsync(int heistId, List<HeistSkillRequirement> updatedSkills);
        Task<HeistSkillRequirement?> GetHeistSkillRequirementAsync(int heistId, int skillId);
        Task<HeistEntity> GetHeistWithRequiredSkillsAsync(int heistId);
    }
}
