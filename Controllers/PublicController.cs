using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX.Controllers
{
    [AllowAnonymous] // Explicitly allow access to everyone, even unauthenticated users
    public class PublicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Add actions for viewing data here
    }
}
