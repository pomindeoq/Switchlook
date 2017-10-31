using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.User
{
    public class UsersResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        
        public IEnumerable<IUserResponseModel> Users { get; set; }
    }
}

