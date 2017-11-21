using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class PointPurchaseTransactionModel
    {
        [Key]
        public int Id { get; set; }
        public Account Account { get; set; }
        public PointTransactionModel Transaction { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
