using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Items
{
    public class ItemResponseModel : IItemResponseModel
    {
        public int ItemId { get; set; }
        public string CategoryName { get; set; }
        public string OwnerUserName { get; set; }
        public double PointValue { get; set; }
    }
}
