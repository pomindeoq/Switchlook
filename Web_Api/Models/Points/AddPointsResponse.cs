using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.Points
{
    public class AddPointsResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public bool Succeeded { get; set; }

        public AddPointsResponse()
        {
            Succeeded = false;
            Errors = null;
        }
    }
}
