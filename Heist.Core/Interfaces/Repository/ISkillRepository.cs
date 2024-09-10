using Heist.Core.Entities;

namespace Heist.Core.Interfaces.Repository
{
    public interface ISkillRepository
    {
        Task<Skill?> GetSkillByNameAsync(string skillName);
        Task<Skill> AddSkillAsync(Skill skill);
        Task<MemberSkill?> GetMemberSkillAsync(int memberId, int skillId);
    }
}
