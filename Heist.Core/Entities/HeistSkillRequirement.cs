using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Entities
{
    public class HeistSkillRequirement
    {
        public int HeistId { get; set; } 
        public HeistEntity Heist { get; set; }

        public int SkillId { get; set; } 
        public Skill Skill { get; set; }

        public string Level { get; set; } 
        public int Members { get; set; } 
    }
}
