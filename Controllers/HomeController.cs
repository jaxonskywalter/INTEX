using INTEX.Models;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


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


        //FILTERING

        [HttpGet]
        public IActionResult Filter(string textileColor, string textileStructure, string sex, int? burialDepth, decimal? estimateStature, string? ageAtDeath, string headDirection, string burialId, string textileFunction, string hairColor)
        {
            var records = context.Burialmain;

            if (!string.IsNullOrEmpty(ageAtDeath))
            {
                records = (DbSet<Burialmain>)records.Where(b => b.Ageatdeath == ageAtDeath);
            }

            if (!string.IsNullOrEmpty(textileStructure))
            {
                records = (DbSet<Burialmain>)records.Where(b => b.Ageatdeath == ageAtDeath);
            }

            var model = new RecordsViewModel
            {
                Records = (IQueryable<Burialmain>)records.ToList()
            };


            return View("Data", model);
        }


        // CONFIRMATION FOR ADDING A RECORD
        //public IActionResult Confirmation()
        //{
        //    return View();
        //}


        [HttpGet]
        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupervisedAnalysis(MummyData data)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://localhost:44375/score");
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                var result = await response.Content.ReadAsStringAsync();
                var prediction = JsonConvert.DeserializeObject<Prediction>(result);
                ViewBag.Prediction = prediction;
                return View();
            }
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