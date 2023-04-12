using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX.Controllers
{
    [Authorize(Roles = "Researcher,Admin")] // Restrict access to Researchers and Admins
    public class ResearchController : Controller
    {
        // Add actions for entering, editing, and deleting data here
    }

    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
