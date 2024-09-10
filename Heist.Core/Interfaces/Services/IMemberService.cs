using Heist.Core.DTO;
using Heist.Core.Entities;

namespace Heist.Core.Interfaces.Services
{
    public interface IMemberService
    {
        Task<CreateMemberResult> CreateMemberAsync(MemberDto memberDto);
        Task<UpdateMemberResult> UpdateMemberSkillsAsync(int id, UpdateMemberSkillDto updateMemberSkillDto);
        Task<bool> RemoveSkillAsync(int memberId, string skillName);
        Task<Member> GetMemberByIdAsync(int id);
    }
}
