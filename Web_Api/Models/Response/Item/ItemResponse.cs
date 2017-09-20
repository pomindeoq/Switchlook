using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response.Item
{
    public class ItemResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public int Id { get; set; }
        public ItemCategory Category { get; set; }
        public string OwnerAccountId { get; set; }
        public double PointValue { get; set; }
    }
}
