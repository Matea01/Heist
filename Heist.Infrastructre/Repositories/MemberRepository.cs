
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


public class MemberRepository : IMemberRepository
{
    private readonly HeistDbContext _dbContext;

    public MemberRepository(HeistDbContext context)
    {
        _dbContext = context;
    }

    public async Task<int> AddMemberAsync(Member member)
    {
        await _dbContext.AddAsync(member);
        await _dbContext.SaveChangesAsync();

        return member.Id; // Return the newly created member's ID
    }

    public async Task UpdateMemberAsync(Member member)
    {
        // Fetch the existing member from the database, including skills
        var existingMember = await _dbContext.Member
            .Include(m => m.Skills) //  include the related Skills
            .FirstOrDefaultAsync(m => m.Id == member.Id);

        if (existingMember == null)
        {
            throw new Exception("Member not found.");
        }

        // Update member's properties
        existingMember.Email = member.Email;
        existingMember.Sex = member.Sex;
        existingMember.Status = member.Status;
        existingMember.MainSkillId = member.MainSkillId;

        // Update the member in the database
        _dbContext.Member.Update(existingMember);
        await _dbContext.SaveChangesAsync();
    }

}
