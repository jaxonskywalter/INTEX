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

        //public IActionResult Data(int pageNum = 1)
        //{
        //    int pageSize = 10; // number of records per page

        //    // calculate the number of records to skip based on the current page number and page size
        //    int skip = (pageNum - 1) * pageSize;

        //    // retrieve a subset of the data from the database using Skip() and Take() methods
        //    var burials = context.Burialmain
        //        .Skip(skip)
        //        .Take(pageSize)
        //        .ToList();

        //    // calculate the total number of pages based on the total number of records and page size
        //    int totalPages = (int)Math.Ceiling(context.Burialmain.Count() / (double)pageSize);

        //    // pass the subset of data and pagination information to the view
        //    ViewBag.CurrentPage = pageNum;
        //    ViewBag.TotalPages = totalPages;

        //    return View(burials);
        //}

        /// SHOW MORE INFO FOR A RECORD
        ///
        //[HttpGet]
        //public IActionResult MoreInfo()
        //{
        //    return View();
        //}


        [HttpGet]
        public IActionResult MoreInfo(long burialid)
        {
            // Retrieve the burial record with the given burialid from the database
            var burialRecord = context.Burialmain.SingleOrDefault(x => x.Id == burialid);

            if (burialRecord == null)
            {
                return NotFound(); // Return 404 error if the burial record is not found
            }

            // Pass the burial record to the AddRecord view so that it can be edited
            return View("MoreInfo", burialRecord);
        }




        //-----------------------------------------------------------FILTERING----------------------------------------------------------------------

        
        public IActionResult Filter(string sex, string burialdepth, string estimatestature, string ageatdeath, string headdirection, string squarenorthsouth, string northsouth, string squareeastwest, string eastwest, string area, string burialnumber, string haircolor, int pageNum = 1)
        {
            var burials = context.Burialmain;

            // Your code to filter records based on the form values goes here
            var filteredRecords = burials.Where(b =>
           (string.IsNullOrEmpty(sex) || b.Sex == sex) && // 
           (string.IsNullOrEmpty(burialdepth) || b.Depth == burialdepth) && //
           (string.IsNullOrEmpty(estimatestature) || b.Length == estimatestature) && //
           (string.IsNullOrEmpty(ageatdeath) || b.Ageatdeath == ageatdeath) && 
           (string.IsNullOrEmpty(headdirection) || b.Headdirection == headdirection) &&
           (string.IsNullOrEmpty(squarenorthsouth) || b.Squarenorthsouth == squarenorthsouth) &&
           (string.IsNullOrEmpty(northsouth) || b.Northsouth == northsouth) &&
           (string.IsNullOrEmpty(squareeastwest) || b.Squareeastwest == squareeastwest) &&
           (string.IsNullOrEmpty(eastwest) || b.Eastwest == eastwest) &&
           (string.IsNullOrEmpty(area) || b.Area == area) &&
           (string.IsNullOrEmpty(burialnumber) || b.Burialnumber == burialnumber) &&
           (string.IsNullOrEmpty(haircolor) || b.Haircolor == haircolor)).ToList();


            int pageSize = 10;

            var viewModel = new RecordsViewModel
            {
                Records = filteredRecords
                .OrderBy(p => p.Id)
                .AsQueryable()
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumRecords = filteredRecords.Count(),
                    RecordsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };





            // Return the filtered records to the BurialRecord view
            return View("Data", viewModel);
        }





        //-----------------------------------------------------------FILTERING----------------------------------------------------------------------

        // ADD RECORD PAGE
        [HttpGet]
        public IActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(Burialmain bm)
        {

            context.Add(bm);
            context.SaveChanges();
            return View("Confirmation", bm);
        }


        [HttpGet]
        public IActionResult Edit(long burialid)
        {
            // Retrieve the burial record with the given burialid from the database
            var burialRecord = context.Burialmain.SingleOrDefault(x => x.Id == burialid);

            if (burialRecord == null)
            {
                return NotFound(); // Return 404 error if the burial record is not found
            }

            // Pass the burial record to the AddRecord view so that it can be edited
            return View(burialRecord);
        }


        [HttpPost]
        public IActionResult Edit(Burialmain bm)
        {
            context.Update(bm);
            context.SaveChanges();

            return RedirectToAction("Data");
        }


        [HttpGet]
        public IActionResult Delete(long burialid)
        {
            var burialRecord = context.Burialmain.SingleOrDefault(x => x.Id == burialid);

            return View(burialRecord);
        }

        [HttpPost]
        public IActionResult Delete(Burialmain bm)
        {
            context.Remove(bm);
            context.SaveChanges();

            return RedirectToAction("Data");
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
