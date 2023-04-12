using INTEX.Models;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //FOR PASSING THE DATABASE STUFF INTO THE ANALYSIS
        private postgresContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, postgresContext temp)
        {
            _logger = logger;
            context = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Data(int pageNum = 1)
        {
            int pageSize = 10;

            // This is how we print off the information for each book
            var x = new RecordsViewModel
            {
                Records = context.Burialmain
                .OrderBy(p => p.Id)
                .AsQueryable()
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),


                PageInfo = new PageInfo
                    {
                        TotalNumRecords = context.Burialmain.Count(),
                        RecordsPerPage = pageSize,
                        CurrentPage = pageNum
                    }
            };

            return View(x);
        }

        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        public IActionResult UnsupervisedAnalysis()
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
