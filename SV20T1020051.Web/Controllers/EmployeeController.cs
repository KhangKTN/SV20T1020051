using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020051.DomainModels;
using SV20T1020051.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020051.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string EMPLOYEE_SEARCH = "employee_search";
        // GET: /<controller>/
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            //Lấy đầu vào tím kiếm hiện đang lưu trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
            //TH session chưa có điều kiện thì tạo mới
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm trong session
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Thêm Nhân viên";
            Employee model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(1990, 1, 1),
                Photo = "nophoto.png"
            };
            return View("Edit", model);
        }


        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật Nhân viên";
            Employee? model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(model.Photo)) model.Photo = "nophoto.png";
            //Console.WriteLine(DateTime.ParseExact(model.BirthDate.ToString().Split(" ")[0], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            //model.BirthDate = DateTime.Parse(model.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            //Console.WriteLine(DateTime.Parse(model.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            return View(model);
        }

        public IActionResult Save(Employee data, string BirthDateInput, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Thêm Nhân viên" : "Cập nhật Nhân viên";
                if (String.IsNullOrWhiteSpace(data.FullName))
                {
                    ModelState.AddModelError("FullName", "Tên không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.BirthDate.ToString()))
                {
                    ModelState.AddModelError("BirthDate", "Ngày sinh không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Address))
                {
                    ModelState.AddModelError("Address", "Địa chỉ không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Phone))
                {
                    ModelState.AddModelError("Phone", "SĐT không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Email))
                {
                    ModelState.AddModelError("Email", "Email không được để trống");
                }
                if (!ModelState.IsValid)
                {
                    return View("Edit", data);
                }

                DateTime? birthDate = BirthDateInput.ToDateTime();
                if (birthDate.HasValue) data.BirthDate = birthDate.Value;
                if(uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images/employees");
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                
                if (data.EmployeeID == 0)
                {
                    int id = CommonDataService.AddEmployee(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu dữ liệu. Vui lòng thử lại sau vài phút");
                return View("Edit", data);
            }
        }

        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.allowDelete = !CommonDataService.IsUsedEmployee(id);
            return View(model);
        }
    }
}