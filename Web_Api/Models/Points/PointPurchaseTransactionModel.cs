using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class PointPurchaseTransactionModel
    {
        public Account Account { get; set; }
        public PointTransactionModel Transaction { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
