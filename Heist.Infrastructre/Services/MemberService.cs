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
        if (memberDto.Skills == null || memberDto.Skills.Count == 0)
        {
            return CreateMemberResult.Failure("At least one skill is required.");
        }

        // Validate each skill to ensure Name is not null or empty
        foreach (var skill in memberDto.Skills)
        {
            if (string.IsNullOrWhiteSpace(skill.Name))
            {
                return CreateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        // Create the new Member object
        var newMember = new Member
        {
            Email = memberDto.Email,
            Name = memberDto.Name,
            Sex = memberDto.Sex,
            Status = memberDto.Status,
            MainSkillId = memberDto.MainSkillId,
            // Assign Skills directly here
            Skills = memberDto.Skills.Select(s => new Skill
            {
                Name = s.Name,
                Level = s.Level,
            }).ToList()
        };

        // Add the new member to the repository (this saves the member and assigns an Id)
        var memberId = await _memberRepository.AddMemberAsync(newMember);

        return CreateMemberResult.Success(memberId);
    }

    public async Task<UpdateMemberResult> UpdateMemberSkillsAsync(UpdateMemberSkillDto updateMemberSkillDto)
    {
        var member = await _memberRepository.GetMemberByIdAsync(updateMemberSkillDto.MemberId);

        if (member == null)
        {
            return UpdateMemberResult.Failure("Member not found");
        }

        // Validate each skill to ensure Name is not null or empty
        foreach (var skill in updateMemberSkillDto.Skills)
        {
            if (string.IsNullOrWhiteSpace(skill.Name))
            {
                return UpdateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        // Update member's skills
        member.Skills = updateMemberSkillDto.Skills.Select(s => new Skill
        {
            Name = s.Name,
            Level = s.Level
        }).ToList();

        // Update the member in the repository
        await _memberRepository.UpdateMemberAsync(member);

        return UpdateMemberResult.Success();
    }
}


