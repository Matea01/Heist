using Heist.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
