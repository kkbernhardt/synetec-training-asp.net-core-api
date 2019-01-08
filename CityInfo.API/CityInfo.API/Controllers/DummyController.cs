using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext ctx;

        public DummyController(CityInfoContext ctx)
        {
            this.ctx = ctx;
        }
        [HttpGet("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}