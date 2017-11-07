using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.User
{
    public class UserResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public IUserResponseModel User { get; set; }
    }
}
