using Heist.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Interfaces.Repository
{
    public interface ISkillRepository
    {
        Task<Skill?> GetSkillByNameAsync(string skillName);
        Task<Skill> AddSkillAsync(Skill skill);
        Task<MemberSkill?> GetMemberSkillAsync(int memberId, int skillId);
    }
}
