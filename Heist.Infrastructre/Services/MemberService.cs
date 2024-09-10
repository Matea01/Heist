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
        var existingMember = await _memberRepository.GetMemberByEmailAsync(memberDto.email);
        if (existingMember != null)
        {
            return CreateMemberResult.Failure("A member with this email already exists.");
        }

        if (memberDto.skills == null || memberDto.skills.Count == 0)
        {
            return CreateMemberResult.Failure("At least one skill is required.");
        }

        foreach (var skill in memberDto.skills)
        {
            if (string.IsNullOrWhiteSpace(skill.name))
            {
                return CreateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        var newMember = new Member
        {
            Email = memberDto.email,
            Name = memberDto.name,
            Sex = memberDto.sex,
            Status = memberDto.status,
            MainSkill = memberDto.mainSkill,
            MemberSkills = new List<MemberSkill>()
        };

        foreach (var skillDto in memberDto.skills)
        {
            var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
            if (skill == null)
            {
                skill = new Skill { Name = skillDto.name };
                await _skillRepository.AddSkillAsync(skill);
            }

            var memberSkill = new MemberSkill
            {
                Skill = skill,
                Level = skillDto.level
            };

            newMember.MemberSkills.Add(memberSkill);
        }

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
        var member = await _memberRepository.GetMemberByIdAsync(memberId);

        if (member == null)
        {
            return false;
        }

        var skillToRemove = member.MemberSkills
            .FirstOrDefault(ms => ms.Skill.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));

        if (skillToRemove == null)
        {
            return false;
        }
        member.MemberSkills.Remove(skillToRemove);

        await _memberRepository.UpdateMemberAsync(member);

        return true;
    }

    public async Task<UpdateMemberResult> UpdateMemberSkillsAsync(int id, UpdateMemberSkillDto updateMemberSkillDto)
    {
        var member = await _memberRepository.GetMemberByIdAsync(id);

        if (member == null)
        {
            return UpdateMemberResult.Failure("Member not found");
        }

        foreach (var skill in updateMemberSkillDto.skills)
        {
            if (string.IsNullOrWhiteSpace(skill.name))
            {
                return UpdateMemberResult.Failure("Each skill must have a valid name.");
            }
        }

        var mainSkillDto = updateMemberSkillDto.skills
            .SingleOrDefault(skill => skill.name.Equals(updateMemberSkillDto.mainSkill, StringComparison.OrdinalIgnoreCase));

        if (mainSkillDto == null)
        {
            return UpdateMemberResult.Failure("When the main skill was changed, but the skill is not part of the member’s previous or\r\nupdated skill array or multiple skills having the same name were provided.");
        }

        foreach (var skillDto in updateMemberSkillDto.skills)
        {
            var skill = await _skillRepository.GetSkillByNameAsync(skillDto.name);
            if (skill == null)
            {
                skill = new Skill { Name = skillDto.name };
                await _skillRepository.AddSkillAsync(skill);
            }

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
        member.MainSkill = updateMemberSkillDto.mainSkill;


        await _memberRepository.UpdateMemberAsync(member);

        return UpdateMemberResult.Success();
    }
}


