using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Interfaces.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        // Private constructor to enforce the use of the factory methods
        private ServiceResult(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        // Factory method for successful results
        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>(true, value, null);
        }

        // Factory method for failure results
        public static ServiceResult<T> Failure(string error)
        {
            return new ServiceResult<T>(false, default, error);
        }
    }
}
