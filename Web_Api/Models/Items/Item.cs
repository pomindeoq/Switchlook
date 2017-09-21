using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;
using WebApi.Models.ItemCategories;

namespace WebApi.Models.Items
{
    public class Item
    {
        public int Id { get; set; }
        public ItemCategory Category { get; set; }
        public Account OwnerAccount { get; set; }
        public double PointValue { get; set; }
    }
}
