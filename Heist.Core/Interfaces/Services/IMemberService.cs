using Heist.Core.DTO;

namespace Heist.Core.Interfaces.Services
{
    public interface IMemberService
    {
        Task<CreateMemberResult> CreateMemberAsync(MemberDto memberDto);
    }
}
