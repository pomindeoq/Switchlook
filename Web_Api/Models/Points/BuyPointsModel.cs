﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Points
{
    public class BuyPointsModel
    {
        public string UserId { get; set; }
        public double Value { get; set; }
        public double Price { get; set; }
    }
}
