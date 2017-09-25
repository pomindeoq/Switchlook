using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models.Points
{
    public class PointsModel
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public double Value { get; set; }

    }
}
