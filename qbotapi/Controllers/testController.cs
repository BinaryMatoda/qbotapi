using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qbotapi.Controllers.ServiceInterfaces;
using qbotapi.Resources;

namespace qbotapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            var now = new DateTime();
            return now.ToString("DD-MM-YYYY HH:mm:ss");
        }
    }
}
