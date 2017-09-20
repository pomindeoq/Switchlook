using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response.Item
{
    public class ItemsResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public IEnumerable<dynamic> Items { get; set; }

    }
}
