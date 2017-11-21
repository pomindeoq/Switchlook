using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.Items
{
    public class CreateItemsResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<int> ItemIds { get; set; }
    }
}
