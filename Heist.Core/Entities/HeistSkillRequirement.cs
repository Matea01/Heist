using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Entities
{
    public class HeistSkillRequirement
    {
        public int HeistId { get; set; } // Foreign key to Heist
        public HeistEntity Heist { get; set; }

        public int SkillId { get; set; } // Foreign key to Skill
        public Skill Skill { get; set; }

        public string Level { get; set; } // Required skill level (e.g., "", "", "**")
        public int Members { get; set; } // Number of members required with this skill
    }
}
