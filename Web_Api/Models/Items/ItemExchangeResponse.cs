using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.Items
{
    public class ItemExchangeResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }

        public ItemExchangeResponse()
        {
            Errors = null;
            Succeeded = false;
        }
    }
}
