
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
    public async Task<Member> AddMemberAsync(Member member)
    {
        await _dbContext.AddAsync(member);
        await _dbContext.SaveChangesAsync();

        return member; // Return the newly created member obj
    }

    public async Task<Member> GetMemberByIdAsync(int memberId)
    {
        // Retrieve the member with their related skills
        return await _dbContext.Member
            .Include(m => m.MemberSkills) // Include skills so they are loaded together with the member
            .ThenInclude(msr => msr.Skill)
            .FirstAsync(m => m.Id == memberId); // Find the member by Id
    }

    public async Task UpdateMemberAsync(Member member)
    {
        // Update the member entity in the context
        _dbContext.Member.Update(member);

        // Save changes to the database
        await _dbContext.SaveChangesAsync();
    }
}
