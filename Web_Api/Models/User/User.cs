using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.User
{
    public class User
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public double Points { get; set; }

    }

}