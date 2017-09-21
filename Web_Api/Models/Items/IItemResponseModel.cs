using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Items
{
    public interface IItemResponseModel
    {
        int ItemId { get; set; }
        string CategoryName { get; set; }
        string OwnerUserName { get; set; }
        double PointValue { get; set; }
    }
}
