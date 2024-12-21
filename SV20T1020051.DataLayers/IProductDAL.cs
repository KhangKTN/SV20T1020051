using System;
using SV20T1020051.DomainModels;

namespace SV20T1020051.DataLayers
{
	public interface IProductDAL
	{
		IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "",
			int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
		int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
		Product? Get(int productID);
		int Add(Product data);
		bool Update(Product data);
		bool Delete(int ProductID);
		bool IsUsed(int ProductID);

		IList<ProductPhoto> ListPhotos(int productID);
		ProductPhoto? GetPhoto(long photoID);
		long AddPhoto(ProductPhoto data);
		bool UpdatePhoto(ProductPhoto data);
		bool DeletePhoto(long photoID);

		IList<ProductAttribute> ListAttributes(int productID);
		ProductAttribute? GetAttribute(long attributeID);
		long AddAttributes(ProductAttribute data);
		bool UpdateAttributes(ProductAttribute data);
		bool DeleteAttributes(long attributeID);
	}
}

