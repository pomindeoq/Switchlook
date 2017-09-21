using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Items;
using WebApi.Models.Response;

namespace WebApi.Models.Items
{
    public class ItemResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public IItemResponseModel Item { get; set; }
    }
}
