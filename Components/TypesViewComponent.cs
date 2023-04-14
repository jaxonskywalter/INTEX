using System;
using System.Linq;
using INTEX.Models;
using Microsoft.AspNetCore.Mvc;

namespace INTEX.Components
{
    public class TypesViewComponent : ViewComponent
    {
        private postgresContext repo { get; set; }

        public TypesViewComponent(postgresContext temp)
        {
            repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Sex = RouteData?.Values["Sex"];

            var types = repo.Burialmain
                .Select(x => x.Sex)
                .Distinct()
                .OrderBy(x => x);

            return View(types);
        }
    }
}




