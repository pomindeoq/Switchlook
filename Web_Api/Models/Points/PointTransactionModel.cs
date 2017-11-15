using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class PointTransactionModel
    {
        public Account Account { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
