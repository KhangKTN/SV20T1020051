
using System;
using SV20T1020051.BusinessLayers;
using SV20T1020051.DataLayers;
using SV20T1020051.DataLayers.MySQL;
using SV20T1020051.DomainModels;

public static class CommonDataService
{
    private static readonly ICommonDAL<Province> provinceDB;
    private static readonly ICommonDAL<Supplier> supplierDB;
    private static readonly ICommonDAL<Customer> customerDB;
    private static readonly ICommonDAL<Shipper> shipperDB;
    private static readonly ICommonDAL<Employee> employeeDB;
    private static readonly ICommonDAL<Category> categoryDB;


    static CommonDataService()
    {
        string connectionString = Configuration.ConnectionString;
        provinceDB = new ProvinceDAL(connectionString);
        customerDB = new CustomerDAL(connectionString);
        categoryDB = new CategoryDAL(connectionString);
        shipperDB = new ShipperDAL(connectionString);
        employeeDB = new EmployeeDAL(connectionString);
        supplierDB = new SupplierDAL(connectionString);
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách khách hàng
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
    {
        rowCount = customerDB.Count(searchValue);
        return customerDB.List(page, pageSize, searchValue).ToList();
    }

    public static List<Customer> ListOfCustomers()
    {
        return customerDB.List().ToList();
    }
    /// <summary>
    /// Lấy thông tin của một khách hàng
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Customer? GetCustomer(int id)
    {
        return customerDB.Get(id);
    }
    /// <summary>
    /// Bổ sung khách hàng mới
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    public static int AddCustomer(Customer customer)
    {
        return customerDB.Add(customer);
    }
    /// <summary>
    /// Cập nhật khách hàng
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    public static bool UpdateCustomer(Customer customer)
    {
        return customerDB.Update(customer);
    }
    /// <summary>
    /// Xoá khách hàng có mã là id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DeleteCustomer(int id)
    {
        if (customerDB.IsUsed(id)) return false;
        return customerDB.Delete(id);
    }
    /// <summary>
    /// Kiểm tra xem khách hàng có mã id hiện có dữ liệu liên quan hay không
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUsedCustomer(int id)
    {
        return customerDB.IsUsed(id);
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách tỉnh thành
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public static List<Province> ListOfProvinces()
    {
        //rowCount = provinceDB.Count(searchValue);
        return provinceDB.List().ToList();
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách Loại hàng
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>g
    public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
    {
        rowCount = categoryDB.Count(searchValue);
        return categoryDB.List(page, pageSize, searchValue).ToList();
    }

    public static List<Category> ListOfCategories(int page = 1, int pageSize = 0, string searchValue = "")
    {
        return categoryDB.List().ToList();
    }
    /// <summary>
    /// Lấy thông tin 1 loại hàng theo mã loại hàng
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Category? GetCategory(int id)
    {
        return categoryDB.Get(id);
    }
    /// <summary>
    /// Thêm mới loại hàng
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static int AddCategory(Category data)
    {
        return categoryDB.Add(data);
    }
    /// <summary>
    /// Cập nhật loại hàng
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateCategory(Category data)
    {
        return categoryDB.Update(data);
    }
    /// <summary>
    /// Xoá loại hàng có mã là id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DeleteCategory(int id)
    {
        return categoryDB.Delete(id);
    }
    /// <summary>
    /// Kiểm tra xem loại hàng có mã id hiện có dữ liệu liên quan hay không
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUsedCategory(int id)
    {
        return categoryDB.IsUsed(id);
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách người giao hàng
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
    {
        rowCount = shipperDB.Count(searchValue);
        return shipperDB.List(page, pageSize, searchValue).ToList();
    }

    public static List<Shipper> ListOfShippers()
    {
        return shipperDB.List().ToList();
    }
    /// <summary>
    /// Lấy thông tin 1 người giao hàng theo mã người giao hàng
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Shipper? GetShipper(int id)
    {
        return shipperDB.Get(id);
    }
    /// <summary>
    /// Thêm mới người giao hàng
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static int AddShipper(Shipper data)
    {
        return shipperDB.Add(data);
    }
    /// <summary>
    /// Cập nhật người giao hàng
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateShipper(Shipper data)
    {
        return shipperDB.Update(data);
    }
    /// <summary>
    /// Xoá người giao hàng có mã là id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DeleteShipper(int id)
    {
        return shipperDB.Delete(id);
    }
    /// <summary>
    /// Kiểm tra người giao hàng có mã id hiện có dữ liệu liên quan hay không
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUsedShipper(int id)
    {
        return shipperDB.IsUsed(id);
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách Nhà cung cấp
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>g
    public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
    {
        rowCount = supplierDB.Count(searchValue);
        return supplierDB.List(page, pageSize, searchValue).ToList();
    }

    public static List<Supplier> ListOfSuppliers(int page = 1, int pageSize = 0, string searchValue = "")
    {
        return supplierDB.List(page, pageSize, searchValue).ToList();
    }
    /// <summary>
    /// Lấy thông tin 1 nhà cung cấp theo mã nhà CC
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Supplier? GetSupplier(int id)
    {
        return supplierDB.Get(id);
    }
    /// <summary>
    /// Thêm mới nhà cung cấp
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static int AddSupplier(Supplier data)
    {
        return supplierDB.Add(data);
    }
    /// <summary>
    /// Cập nhật nhà cung cấp
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateSupplier(Supplier data)
    {
        return supplierDB.Update(data);
    }
    /// <summary>
    /// Xoá nhà cung cấp có mã là id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DeleteSupplier(int id)
    {
        return supplierDB.Delete(id);
    }
    /// <summary>
    /// Kiểm tra xem nhà cung cấp có mã id hiện có dữ liệu liên quan hay không
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUsedSupplier(int id)
    {
        return supplierDB.IsUsed(id);
    }

    /// <summary>
    /// Tìm kiếm và lấy danh sách nhân viên
    /// </summary>
    /// <param name="rowCount"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <returns></returns>
    public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
    {
        rowCount = employeeDB.Count(searchValue);
        return employeeDB.List(page, pageSize, searchValue).ToList();
    }
    /// <summary>
    /// Lấy thông tin 1 nhân viên theo mã nhân viên
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Employee? GetEmployee(int id)
    {
        return employeeDB.Get(id);
    }
    /// <summary>
    /// Thêm mới nhân viên
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static int AddEmployee(Employee data)
    {
        return employeeDB.Add(data);
    }
    /// <summary>
    /// Cập nhật nhân viên
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateEmployee(Employee data)
    {
        return employeeDB.Update(data);
    }
    /// <summary>
    /// Xoá nhân viên có mã là id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DeleteEmployee(int id)
    {
        return employeeDB.Delete(id);
    }
    /// <summary>
    /// Kiểm tra xem nhân viên có mã id hiện có dữ liệu liên quan hay không
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUsedEmployee(int id)
    {
        return employeeDB.IsUsed(id);
    }
}