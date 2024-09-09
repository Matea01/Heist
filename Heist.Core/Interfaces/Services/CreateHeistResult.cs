using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Interfaces.Services
{
    public class CreateHeistResult
    {
        public bool IsSuccess { get; private set; }
        public string[] Errors { get; private set; }
        public object HeistId { get; set; }

        public static CreateHeistResult Success(int heistId) =>
           new CreateHeistResult { IsSuccess = true, HeistId = heistId };

        public static CreateHeistResult Failure(params string[] errors) =>
            new CreateHeistResult { IsSuccess = false, Errors = errors };
    }
}

