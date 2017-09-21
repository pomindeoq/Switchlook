﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Accounts;

namespace WebApi.Models
{
    public class Points
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public double Value { get; set; }
    }
}
