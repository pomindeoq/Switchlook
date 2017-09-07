using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Models.Response
{
    public class ExternalLoginResponse : LoginResponse
    {     
        public IdentityResult CreateResult { get; set; }
        public bool IsRegistered { get; set; }

        public ExternalLoginResponse()
        {
            Errors = null;
            IsModelValid = false;
            Result = null;
            CreateResult = null;
            IsRegistered = false;
        }
    }
}
