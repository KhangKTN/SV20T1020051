using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020051.Web.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: /<controller>/
        [Route("/Error/{statusCode}")]
        public IActionResult ErrorRoute(string statusCode)
        {
            //if (statusCode == "500" | statusCode == "404")
            //{
            //    return View(statusCode);
            //}
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}

