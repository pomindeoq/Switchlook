using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet, Route("hey")]
        public string Test()
        {
           

            return "hey1";
        }

    // GET api/values
        [HttpGet, Route("GetTest")]
        public IEnumerable<string> Get()
        {
            
            return new string[] { "value1", "value2" + User.Identity.IsAuthenticated };
        }

        [HttpGet, Route("SetGet/number={testNumber}&string={testString}")]
        public IEnumerable<string> SetGet(int testNumber, string testString)
        {
            
            return new string[] { "value1" + testString, "value2" + testNumber };
        }
    }
}
