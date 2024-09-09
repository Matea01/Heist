using Heist.Core.DTO;
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;

namespace Heist.Core.Interfaces.Services
{
    public class HeistService : IHeistService
    {
        private readonly IHeistRepository _heistRepository;
        private readonly ISkillRepository _skillRepository;

        public HeistService(IHeistRepository heistRepository, ISkillRepository skillRepository)
        {
            _heistRepository = heistRepository;
            _skillRepository = skillRepository;
        }

        public async Task<CreateHeistResult> CreateHeistAsync(AddHeistDto heistDto)
        {

            // Check if a heist with the same name already exists
            var existingHeist = await _heistRepository.GetHeistByNameAsync(heistDto.name);
            if (existingHeist != null)
            {
                return CreateHeistResult.Failure("A heist with this name already exists.");
            }

            // Validate startTime and endTime
            if (heistDto.startTime >= heistDto.endTime)
            {
                return CreateHeistResult.Failure("The start time must be before the end time.");
            }
            if (heistDto.endTime <= DateTime.UtcNow)
            {
                return CreateHeistResult.Failure("The end time cannot be in the past.");
            }

            //Check if heistDto is null or name is ""
            if (heistDto == null || string.IsNullOrEmpty(heistDto.name) || heistDto.skills == null)
            {
                throw new ArgumentException("Heist data is invalid.");
            }

            // Validate skills for duplicates
            var skillSet = new HashSet<(string Name, string Level)>();
            foreach (var skillDto in heistDto.skills)
            {
                var skillKey = (skillDto.name, skillDto.level);
                if (!skillSet.Add(skillKey))
                {
                    return CreateHeistResult.Failure($"Duplicate skill: {skillDto.name} with level {skillDto.level}.");
                }

                // Check if the skill exists, if not create it
                var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
                if (skill == null)
                {
                    skill = new Skill { Name = skillDto.name };
                    await _skillRepository.AddSkillAsync(skill);
                }

                // Create the heist if all validations pass
                var heist = new HeistEntity
                {
                    Name = heistDto.name,
                    Location = heistDto.location,
                    StartTime = heistDto.startTime,
                    EndTime = heistDto.endTime,
                    SkillRequirements = new List<HeistSkillRequirement>()
                };

                // Add the HeistSkillRequirement
                heist.SkillRequirements.Add(new HeistSkillRequirement
                {
                    Skill = skill,
                    Level = skillDto.level,
                    Members = skillDto.members
                });
            }


            await _heistRepository.AddHeistAsync(heist);

            return CreateHeistResult.Success(heist.Id);
        }
    }

}
