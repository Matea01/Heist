using Heist.Core.DTO;
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }
    public async Task<CreateMemberResult> CreateMemberAsync(MemberDto memberDto)
    {
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

        // Create the new Member object (mapping)
        var newMember = new Member
        {
            Email = memberDto.email,
            Name = memberDto.name,
            Sex = memberDto.sex,
            Status = memberDto.status,
            MainSkill = memberDto.mainSkill,
            // Assign Skills directly here
            Skills = memberDto.skills.Select(s => new Skill
            {
                Name = s.name,
                Level = s.level,
            }).ToList()
        };

        // Add the new member to the repository (this saves the member and assigns an Id)
        var member = await _memberRepository.AddMemberAsync(newMember);
        //

        return CreateMemberResult.Success(member.Id);
    }

    public async Task<bool> RemoveSkillAsync(int memberId, string skillName)
    {
        // Get the member 
        var member = await _memberRepository.GetMemberByIdAsync(memberId);

        if (member == null)
        {
            return false;  
        }

        // Find the skill within the member's skill list
        var skillToRemove = member.Skills.FirstOrDefault(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));

        if (skillToRemove == null)
        {
            return false;  // Skill not found
        }

        // Remove the skill from the member's skill list
        member.Skills.Remove(skillToRemove);

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
        var mainSkill = updateMemberSkillDto.skills.SingleOrDefault(skill => skill.name.Equals(updateMemberSkillDto.mainSkill, StringComparison.OrdinalIgnoreCase));

        if (mainSkill == null)
        {
            return UpdateMemberResult.Failure("The specified main skill does not exist in the skills list.");
        }

        // Validate each skill to ensure Name is not null or empty
        foreach (var skill in updateMemberSkillDto.skills)
        {
            if (string.IsNullOrWhiteSpace(skill.name))
            {
                return UpdateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        // Update member's skills
        member.Skills = updateMemberSkillDto.skills.Select(s => new Skill
        {
            Name = s.name,
            Level = s.level
        }).ToList();
        member.MainSkill = updateMemberSkillDto.mainSkill;
        // Update the member.mainSkill in the repository
        await _memberRepository.UpdateMemberAsync(member);

        return UpdateMemberResult.Success();
    }
}


