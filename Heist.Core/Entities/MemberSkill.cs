using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Entities
{
    public class MemberSkill
    {
        public int MemberId { get; set; } 
        public Member Member { get; set; } 

        public int SkillId { get; set; } 
        public Skill Skill { get; set; } 

        public string Level { get; set; } 
    }
}
