using Heist.Core.DTO;

namespace Heist.Core.Interfaces.Services
{
    public interface IHeistService
    {
        Task<CreateHeistResult> CreateHeistAsync(AddHeistDto heistDto);
        Task<ServiceResult<List<MemberDto>>> GetEligibleMembersAsync(int heistId);
        Task<UpdateHeistResult> UpdateHeistSkillsAsync(int id, UpdateHeistSkillsDto updateHeistSkillsDto);
    }
}