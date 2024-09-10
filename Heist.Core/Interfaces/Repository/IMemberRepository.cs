using Heist.Core.Entities;

namespace Heist.Core.Interfaces.Repository
{
    public interface IMemberRepository
    {
        Task<Member> AddMemberAsync(Member member);
        Task<Member> GetMemberByEmailAsync(string email);
        Task<Member?> GetMemberByIdAsync(int memberId);
        Task UpdateMemberAsync(Member member);
    }
}
