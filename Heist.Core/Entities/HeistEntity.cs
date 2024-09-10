namespace Heist.Core.Entities
{
    public class HeistEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // A collection of skills required for the heist
        public List<HeistSkillRequirement> SkillRequirements { get; set; } = new List<HeistSkillRequirement>();
    }
}
