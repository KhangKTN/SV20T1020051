using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020051.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: /<controller>/
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Phan Dinh Khang",
                BirthDate = DateTime.Now,
                Salary = 10.25m
            };
            return View(model);
        }

        public IActionResult Save(Models.Person model, string BirthDateInput = "")
        {
            return Json(model);
        }
    }
}

