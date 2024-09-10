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

            var existingHeist = await _heistRepository.GetHeistByNameAsync(heistDto.name);
            if (existingHeist != null)
            {
                return CreateHeistResult.Failure("A heist with this name already exists.");
            }

            if (heistDto.startTime >= heistDto.endTime)
            {
                return CreateHeistResult.Failure("The start time must be before the end time.");
            }
            if (heistDto.endTime <= DateTime.UtcNow)
            {
                return CreateHeistResult.Failure("The end time cannot be in the past.");
            }

            if (heistDto == null || string.IsNullOrEmpty(heistDto.name) || heistDto.skills == null)
            {
                throw new ArgumentException("Heist data is invalid.");
            }

            var skillSet = new HashSet<(string Name, string Level)>();
            foreach (var skillDto in heistDto.skills)
            {
                var skillKey = (skillDto.name, skillDto.level);
                if (!skillSet.Add(skillKey))
                {
                    return CreateHeistResult.Failure($"Duplicate skill: {skillDto.name} with level {skillDto.level}.");
                }
            }
            var heist = new HeistEntity
            {
                Name = heistDto.name,
                Location = heistDto.location,
                StartTime = heistDto.startTime,
                EndTime = heistDto.endTime,
                SkillRequirements = new List<HeistSkillRequirement>()
            };
            foreach (var skillDto in heistDto.skills)
            {
                var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
                if (skill == null)
                {
                    skill = new Skill { Name = skillDto.name };
                    await _skillRepository.AddSkillAsync(skill);
                }

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

        public async Task<UpdateHeistResult> UpdateHeistSkillsAsync(int heistId, UpdateHeistSkillsDto updateHeistSkillsDto)
        {
            var heist = await _heistRepository.GetHeistByIdAsync(heistId);
            if (heist == null)
            {
                return UpdateHeistResult.Failure("Heist not found", 404);
            }

            if (heist.StartTime <= DateTime.UtcNow)
            {
                return UpdateHeistResult.Failure("The heist has already started", 405);
            }

            foreach (var skill in updateHeistSkillsDto.Skills)
            {
                if (string.IsNullOrWhiteSpace(skill.name))
                {
                    return UpdateHeistResult.Failure("Each skill must have a valid name.", 400);
                }
            }

            var skillGroups = updateHeistSkillsDto.Skills
                .GroupBy(s => new { s.name, s.level })
                .ToList();

            if (skillGroups.Any(g => g.Count() > 1))
            {
                return UpdateHeistResult.Failure("Multiple skills with the same name and level were provided.", 400);
            }

            var updatedSkills = new List<HeistSkillRequirement>();

            foreach (var skillDto in updateHeistSkillsDto.Skills)
            {
                var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
                if (skill == null)
                {
                    skill = new Skill { Name = skillDto.name };
                    await _skillRepository.AddSkillAsync(skill);
                }

                var heistSkill = await _heistRepository.GetHeistSkillRequirementAsync(heistId, skill.Id);
                if (heistSkill == null)
                {
                    heistSkill = new HeistSkillRequirement
                    {
                        HeistId = heistId,
                        SkillId = skill.Id,
                        Skill = skill,
                    };
                }
                heistSkill.Level = skillDto.level;
                

                updatedSkills.Add(heistSkill);
            }

            heist.SkillRequirements = updatedSkills;

            await _heistRepository.UpdateHeistAsync(heist);

            return UpdateHeistResult.Success();
        }
    }

}


