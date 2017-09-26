using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Items
{
    public class ItemExchangeModel
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public Account OldOwnerAccount { get; set; }
        public Account NewOwnerAccount { get; set; }
        public double PointValue { get; set; }
    }
}
