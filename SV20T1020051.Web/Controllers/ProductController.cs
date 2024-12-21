using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020051.BusinessLayers;
using SV20T1020051.DomainModels;
using SV20T1020051.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SV20T1020051.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        // GET: /<controller>/
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search";
        // GET: /<controller>/
        public IActionResult Index(int page = 1, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            //int rowCount = 0;
            //var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue, categoryID, supplierID, minPrice, maxPrice);
            //var model = new Models.ProductSearchResult()
            //{
            //    Page = page,
            //    PageSize = PAGE_SIZE,
            //    SearchValue = searchValue ?? "",
            //    RowCount = rowCount,
            //    Data = data
            //};
            //return View(model);
            ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            //TH session chưa có điều kiện thì tạo mới
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                    //CategoryID = 0,
                    //SupplierID = 0
                };
            }
            //Console.WriteLine("Pagesize Product: ", input.PageSize);
            return View(input);
        }

        public IActionResult Search(ProductSearchInput input)
        {
            //Console.WriteLine("Pagesize Product: ", input.PageSize);
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            //Lưu lại điều kiện tìm kiếm trong session
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Mặt hàng";
            ViewBag.IsEdit = false;
            Product model = new Product()
            {
                ProductID = 0,
                Photo = "macbook.jpeg"
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật Thông tin mặt hàng";
            ViewBag.IsEdit = true;
            Product? model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(model.Photo)) model.Photo = "macbook.jpeg";
            return View(model);
        }

        public IActionResult Save(Product data, string BirthDateInput, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                if (String.IsNullOrWhiteSpace(data.ProductName))
                {
                    ModelState.AddModelError("ProductName", "Tên không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.CategoryID.ToString()))
                {
                    ModelState.AddModelError("CategoryID", "Loại hàng không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.SupplierID.ToString()))
                {
                    ModelState.AddModelError("SupplierID", "Nhà cung cấp không được để trống");
                }
                if (String.IsNullOrWhiteSpace(data.Unit))
                {
                    ModelState.AddModelError("Unit", "Đơn vị không được để trống");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.IsEdit = data.ProductID == 0 ? false : true;
                    return View("Edit", data);
                }

                DateTime? birthDate = BirthDateInput.ToDateTime();
                if (uploadPhoto != null)
                {
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images/products");
                    string filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IsEdit = data.ProductID == 0 ? false : true;
                ModelState.AddModelError("Error", "Không thể lưu dữ liệu. Vui lòng thử lại sau vài phút");
                return View("Edit", data);
            }
        }

        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.allowDelete = !ProductDataService.IsUsedProduct(id);
            return View(model);
        }

        public IActionResult Photo(IFormFile? uploadPhoto, ProductPhoto data, int id, String method, long photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    ProductPhoto model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id,
                        Photo = "macbook.jpeg"
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    ProductPhoto? modelPhoto = ProductDataService.GetPhoto(photoId);
                    if (modelPhoto == null)
                    {
                        return RedirectToAction("Index");
                    }
                    if (string.IsNullOrEmpty(modelPhoto.Photo)) modelPhoto.Photo = "macbook.jpeg";
                    return View(modelPhoto);
                case "save":
                    try
                    {
                        if (uploadPhoto != null)
                        {
                            string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                            string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images/products");
                            string filePath = Path.Combine(folder, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                uploadPhoto.CopyTo(stream);
                            }
                            data.Photo = fileName;
                        }
                        if (data.PhotoID == 0)
                        {
                            long idAdd = ProductDataService.AddPhoto(data);
                        }
                        else
                        {
                            bool result = ProductDataService.UpdatePhoto(data);
                        }
                        return RedirectToAction("Edit", new {id = id});
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                case "delete":
                    bool delete = ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Attribute(ProductAttribute data, int id, String method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    ProductAttribute model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    ProductAttribute? modelAttribute = ProductDataService.GetAttribute(attributeId);
                    if (modelAttribute == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(modelAttribute);
                case "save":
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung thuộc tính" : "Cập nhật thuộc tính";
                    if (String.IsNullOrWhiteSpace(data.AttributeName))
                    {
                        ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
                    }
                    if (String.IsNullOrWhiteSpace(data.AttributeValue))
                    {
                        ModelState.AddModelError("AttributeValue", "Giá trị không được để trống");
                    }
                    if (String.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
                    {
                        ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
                    }
                    if (!ModelState.IsValid)
                    {
                        //ViewBag.IsEdit = data.AttributeID == 0 ? false : true;
                        return View("Attribute", data);
                    }

                    if (data.AttributeID == 0)
                    {
                        long idAdd = ProductDataService.AddAttribute(data);
                    }
                    else
                    {
                        bool result = ProductDataService.UpdateAttribute(data);
                    }
                    return RedirectToAction("Edit", new { id = id });
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}

