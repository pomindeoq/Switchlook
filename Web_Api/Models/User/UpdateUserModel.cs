using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.User
{
    public class UpdateUserModel
    {
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public double UserPoints { get; set; }
    }
}
