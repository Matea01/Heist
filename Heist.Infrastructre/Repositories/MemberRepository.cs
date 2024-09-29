
using Heist.Core.Entities;
using Heist.Core.Interfaces.Repository;
using Heist.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


public class MemberRepository : IMemberRepository
{
    private readonly HeistDbContext _dbContext;

    public MemberRepository(HeistDbContext context)
    {
        _dbContext = context;
    }
    public async Task<Member> AddMemberAsync(Member member)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.Member.AddAsync(member);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                throw;
            }
        }
        return member;
    }

    public async Task<Member> GetMemberByEmailAsync(string email)
    {
        return await _dbContext.Member.FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<Member?> GetMemberByIdAsync(int memberId)
    {
        return await _dbContext.Member
            .Include(m => m.MemberSkills)
            .ThenInclude(msr => msr.Skill)
            .FirstOrDefaultAsync(m => m.Id == memberId);
    }

    public async Task UpdateMemberAsync(Member member)
    {
        _dbContext.Member.Update(member);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<List<Member>> GetMembersAsync(Expression<Func<Member, bool>> predicate)
    {
        return await _dbContext.Member
            .Include(m => m.MemberSkills)
            .Where(predicate)
            .ToListAsync();
    }


}
