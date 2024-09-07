using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Interfaces.Services
{
    public class UpdateMemberResult
    {
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }

        public static UpdateMemberResult Success()
        {
            return new UpdateMemberResult { IsSuccess = true };
        }

        public static UpdateMemberResult Failure(params string[] errors)
        {
            return new UpdateMemberResult { IsSuccess = false, Errors = errors };
        }
    }
}
