
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


}
