using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020051.DomainModels;
using SV20T1020051.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020051.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SUPPLIER_SEARCH = "supplier_search";
        // GET: /<controller>/
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            //Lấy đầu vào tím kiếm hiện đang lưu trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH);
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
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new SupplierSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm trong session
            ApplicationContext.SetSessionData(SUPPLIER_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Thêm nhà cung cấp";
            Supplier model = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            Supplier? model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Supplier data)
        {
            try
            {
                //Kiểm soát đầu vào và đưa TB lỗi vào ModelState
                if (String.IsNullOrWhiteSpace(data.SupplierName))
                {
                    ModelState.AddModelError("SupplierName", "Tên không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.ContactName))
                {
                    ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
                }

                if (String.IsNullOrWhiteSpace(data.Phone))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Email))
                {
                    ModelState.AddModelError("Email", "Email không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Address))
                {
                    ModelState.AddModelError("Address", "Địa chỉ không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Provice))
                {
                    ModelState.AddModelError("Provice", "Tỉnh thành không được để trống");
                }

                //Thông qua isValid của ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                    return View("Edit", data);
                }
                if (data.SupplierID == 0)
                {
                    int id = CommonDataService.AddSupplier(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateSupplier(data);
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
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.allowDelete = !CommonDataService.IsUsedSupplier(id);
            return View(model);
        }
    }
}

