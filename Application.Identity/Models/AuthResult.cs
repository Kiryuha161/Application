using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public User? User { get; set; }
        public string? Error { get; set; }

        public static AuthResult Success(string token, User user) => new AuthResult
        {
            IsSuccess = true,
            Token = token,
            User = user
        };

        public static AuthResult Fail(string error) => new AuthResult
        {
            IsSuccess = false,
            Error = error
        };
    }
}
