using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class PointTransactionModel
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public double PreviousAmount { get; set; }
        public double NewAmount { get; set; }
        public DateTime DateTime { get; set; }
        [NotMapped]
        public double Amount
        {
            get
            {
                return PreviousAmount > NewAmount ? (PreviousAmount - NewAmount)*-1 : NewAmount - PreviousAmount;
            }
        }

    }
}
