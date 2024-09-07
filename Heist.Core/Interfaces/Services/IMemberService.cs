using Heist.Core.DTO;

namespace Heist.Core.Interfaces.Services
{
    public interface IMemberService
    {
        Task<CreateMemberResult> CreateMemberAsync(MemberDto memberDto);
        Task<UpdateMemberResult> UpdateMemberSkillsAsync(int id, UpdateMemberSkillDto updateMemberSkillDto);
        Task<bool> RemoveSkillAsync(int memberId, string skillName);
    }
}
