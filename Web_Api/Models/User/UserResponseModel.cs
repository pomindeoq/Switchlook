using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.User
{
    public class UserResponseModel : IUserResponseModel
    {
       public string UserID { get; set; }

       public string UserName { get; set; }

       public  string UserEmail { get; set; }

       public double UserPoints { get; set; }
    }
}
