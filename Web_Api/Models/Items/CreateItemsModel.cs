using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Items
{
    public class CreateItemsModel
    {
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public List<double> PointValues { get; set; }
    }
}
