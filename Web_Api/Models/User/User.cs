using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.User
{
    public class User
    {
        public Account ID { get; set; }

        public Account Name { get; set; }

        public Account Email { get; set; }

        public double Points { get; set; }

    }

}