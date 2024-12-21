using System;
using SV20T1020051.DomainModels;

namespace SV20T1020051.Web.Models
{
	public abstract class BasePaginationResult
	{
		public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 1;
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    return c += 1;
                }
                return c;
            }
        }
    }

    public class CustomerSearchResult: BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }

    public class CategorySearchResult : BasePaginationResult
    {
        public List<Category> Data { get; set; }
    }

    public class ShipperSearchResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }

    public class SupplierSearchResult : BasePaginationResult
    {
        public List<Supplier> Data { get; set; }
    }

    public class EmployeeeSearchResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }

    public class ProductSearchResult : BasePaginationResult
    {
        public List<Product> Data { get; set; }
    }
}

