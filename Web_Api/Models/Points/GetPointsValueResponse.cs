using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.Points
{
    public class GetPointsValueResponse : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public double Points { get; set; }
    }
}
