﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response
{
    public class Response : IResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
