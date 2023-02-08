using Gedik.ImageClassification.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gedik.ImageClassification.Service.Controllers
{
    public class TestController : Controller
    {
        [Route("v1/test/ping")]
        [HttpGet]
        public ServiceResult<string> Index()
        {
            return new ServiceResult<string>() { Data = "Pong", Message = "", Status=true };
        }
    }
}
