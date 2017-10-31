using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.User
{
    public interface IUserResponseModel
    {
        string UserName { get; set; }

        string UserEmail { get; set; }

        double UserPoints { get; set; }
    }
}
