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
        Task<int> AddMemberAsync(Member member);
        Task UpdateMemberAsync(Member newMember);
    }
}
