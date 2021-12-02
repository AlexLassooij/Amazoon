using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mongoTest.Models;
using Microsoft.Extensions.Configuration;

namespace mongoTest.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult Home(
            [FromQuery] int rows,
            [FromQuery] int cols)
        {
            return View();

            //var data = connection.Query<SqlTest>("select * from Robots ");
            //Console.WriteLine(data.ToList());
            //return Ok(rows + "\t" + cols + "\n" + data);
            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
