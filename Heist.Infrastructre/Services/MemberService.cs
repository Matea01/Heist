using Heist.Core.DTO;
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ISkillRepository _skillRepository;

    public MemberService(IMemberRepository memberRepository, ISkillRepository skillRepositoty)
    {
        _memberRepository = memberRepository;
        _skillRepository = skillRepositoty;
    }
    public async Task<CreateMemberResult> CreateMemberAsync(MemberDto memberDto)
    {
        // Check if the email is already in use
        var existingMember = await _memberRepository.GetMemberByEmailAsync(memberDto.email);
        if (existingMember != null)
        {
            return CreateMemberResult.Failure("A member with this email already exists.");
        }

        // Check if at least one skill is provided
        if (memberDto.skills == null || memberDto.skills.Count == 0)
        {
            return CreateMemberResult.Failure("At least one skill is required.");
        }

        // Validate each skill to ensure Name is not null or empty
        foreach (var skill in memberDto.skills)
        {
            if (string.IsNullOrWhiteSpace(skill.name))
            {
                return CreateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        // Create a new Member object
        var newMember = new Member
        {
            Email = memberDto.email,
            Name = memberDto.name,
            Sex = memberDto.sex,
            Status = memberDto.status,
            MainSkill = memberDto.mainSkill,
            MemberSkills = new List<MemberSkill>() // Initialize the list for MemberSkills
        };


        // Process each skill in the memberDto
        foreach (var skillDto in memberDto.skills)
        {
            // Check if the skill already exists in the repository
            var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
            if (skill == null)
            {
                // If the skill does not exist, create a new Skill
                skill = new Skill { Name = skillDto.name };
                await _skillRepository.AddSkillAsync(skill);
            }

            // Create the MemberSkill association and add it to the MemberSkills list
            var memberSkill = new MemberSkill
            {
                Skill = skill,
                Level = skillDto.level // Set the skill level for the member
            };

            newMember.MemberSkills.Add(memberSkill);
        }

        // Add the new member to the repository (this saves the member and assigns an Id)
        var member = await _memberRepository.AddMemberAsync(newMember);

        return CreateMemberResult.Success(member.Id);
    }

    public async Task<Member> GetMemberByIdAsync(int id)
    {
        var member = await _memberRepository.GetMemberByIdAsync(id);
        return member;
    }

    public async Task<bool> RemoveSkillAsync(int memberId, string skillName)
    {
        // Get the member 
        var member = await _memberRepository.GetMemberByIdAsync(memberId);

        if (member == null)
        {
            return false; //member not found 
        }

        // Find the skill to remove within the member's skills
        var skillToRemove = member.MemberSkills
            .FirstOrDefault(ms => ms.Skill.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));

        if (skillToRemove == null)
        {
            return false;  // Skill not found in the member's skills
        }
        // Remove the skill from the member's MemberSkills list
        member.MemberSkills.Remove(skillToRemove);
        // Update the member after modification
        await _memberRepository.UpdateMemberAsync(member);

        return true;  // Skill removed successfully
    }

    public async Task<UpdateMemberResult> UpdateMemberSkillsAsync(int id, UpdateMemberSkillDto updateMemberSkillDto)
    {
        var member = await _memberRepository.GetMemberByIdAsync(id);

        if (member == null)
        {
            return UpdateMemberResult.Failure("Member not found");
        }

        // Validate each skill to ensure Name is not null or empty
        foreach (var skill in updateMemberSkillDto.skills)
        {
            if (string.IsNullOrWhiteSpace(skill.name))
            {
                return UpdateMemberResult.Failure("Each skill must have a valid name.");
            }
        }
        // Check if the main skill exists in the skills list
        var mainSkillDto = updateMemberSkillDto.skills
            .SingleOrDefault(skill => skill.name.Equals(updateMemberSkillDto.mainSkill, StringComparison.OrdinalIgnoreCase));

        if (mainSkillDto == null)
        {
            return UpdateMemberResult.Failure("When the main skill was changed, but the skill is not part of the member’s previous or\r\nupdated skill array or multiple skills having the same name were provided.");
        }

        foreach (var skillDto in updateMemberSkillDto.skills)
        {
            // Check if the skill exists in the repository, if not, create it
            var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
            if (skill == null)
            {
                skill = new Skill { Name = skillDto.name };
                await _skillRepository.AddSkillAsync(skill);
            }

            // Create the MemberSkill association and add it to the MemberSkills list
            var memberSkill = await _skillRepository.GetMemberSkillAsync(id, skill.Id);
            if (memberSkill == null)
            {
                memberSkill = new MemberSkill
                {
                    MemberId = id,
                    Skill = skill,
                };
            }
            memberSkill.Level = skillDto.level; // Set the skill level for the member

            member.MemberSkills.Add(memberSkill);
        }
        // Set the main skill of the member
        member.MainSkill = updateMemberSkillDto.mainSkill;

        // Update the member in the repository
        await _memberRepository.UpdateMemberAsync(member);

        return UpdateMemberResult.Success();
    }
}


