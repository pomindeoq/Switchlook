﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Items
{
    public class ItemExchangeViewModel
    {
        public int ItemId { get; set; }
        public string NewOwnerAccountId { get; set; }
        public double PointValue { get; set; }
    }
}
