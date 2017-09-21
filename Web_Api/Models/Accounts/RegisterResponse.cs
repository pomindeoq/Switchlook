using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApi.Models.Response;

namespace WebApi.Models.Accounts
{
    public class RegisterResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public bool IsModelValid { get; set; }
        public bool Succeeded { get; set; }

        public RegisterResponse()
        {
            Errors = null;
            IsModelValid = false;
            Succeeded = false;
        }
    }
}
