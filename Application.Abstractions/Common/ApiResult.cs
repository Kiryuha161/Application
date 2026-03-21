using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        public static ApiResult<T> Success(T data) => new()
        {
            IsSuccess = true,
            Data = data
        };

        public static ApiResult<T> Fail(string error) => new()
        {
            IsSuccess = false,
            Error = error
        };
    }
}
