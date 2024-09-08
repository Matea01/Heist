using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.DTO
{
    public class AddHeistDto
    {
        public string name { get; set; } 
        public string location { get; set; }
        public DateTime startTime { get; set; } 
        public DateTime endTime { get; set; } 
        public List<SkillDto> skills { get; set; } = new List<SkillDto>(); 
    }
}
