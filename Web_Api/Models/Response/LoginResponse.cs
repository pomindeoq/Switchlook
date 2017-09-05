using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Models.Response
{
    public class LoginResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public bool IsModelValid { get; set; }
        public SignInResult Result { get; set; }

        public LoginResponse()
        {
            Errors = null;
            IsModelValid = false;
            Result = null;
        }
    }
}
