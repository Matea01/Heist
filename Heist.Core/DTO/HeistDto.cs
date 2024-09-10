namespace Heist.Core.DTO
{
    public class AddHeistDto
    {
        public required string name { get; set; }
        public required string location { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public List<HeistSkillDto> skills { get; set; } = new List<HeistSkillDto>();
    }
    public class HeistSkillRequirementDto
    {
        public string skillName { get; set; }
        public string level { get; set; }
        public int members { get; set; }
    }
}
