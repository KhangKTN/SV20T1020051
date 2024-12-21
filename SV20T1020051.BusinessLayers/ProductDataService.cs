﻿using System;
using Azure;
using System.Drawing.Printing;
using SV20T1020051.DataLayers;
using SV20T1020051.DataLayers.MySQL;
using SV20T1020051.DomainModels;
using SV20T1020051.BusinessLayers;

public class ProductDataService
{
	private static readonly IProductDAL productDB;

	static ProductDataService()
	{
		productDB = new ProductDAL(Configuration.ConnectionString);
	}

	//public static List<Product> ListProducts(string searchValue = "")
	//{
	//	//return productDB.List(searchValue).ToList();
	//}

	public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
	{
        rowCount = productDB.Count(searchValue, categoryID, supplierID, minPrice, maxPrice);
        return productDB.List(page, pageSize, searchValue, categoryID, supplierID, minPrice, maxPrice).ToList();
    }

	public static Product? GetProduct(int productID)
	{
		return productDB.Get(productID);
	}

	public static int AddProduct(Product data)
	{
		return productDB.Add(data);
	}

	public static bool UpdateProduct(Product data)
	{
		return productDB.Update(data);
	}

	public static bool DeleteProduct(int productID)
	{
		return productDB.Delete(productID);
	}

	public static bool IsUsedProduct(int productID)
	{
		return productDB.IsUsed(productID);
	}

	public static List<ProductPhoto> ListPhotos(int productID)
	{
		return (List<ProductPhoto>)productDB.ListPhotos(productID);
	}

	public static ProductPhoto? GetPhoto(long photoID)
	{
		return productDB.GetPhoto(photoID);
	}

	public static long AddPhoto(ProductPhoto data)
	{
		return productDB.AddPhoto(data);
	}

	public static bool UpdatePhoto(ProductPhoto data)
	{
		return productDB.UpdatePhoto(data);
	}

	public static bool DeletePhoto(long photoID)
	{
		return productDB.DeletePhoto(photoID);
	}

	public static List<ProductAttribute> ListAttributes(int productID)
	{
		return (List<ProductAttribute>)productDB.ListAttributes(productID);
	}

    public static ProductAttribute? GetAttribute(int attributeID)
    {
        return productDB.GetAttribute(attributeID);
    }

    public static long AddAttribute(ProductAttribute data)
    {
        return productDB.AddAttributes(data);
    }

    public static bool UpdateAttribute(ProductAttribute data)
    {
        return productDB.UpdateAttributes(data);
    }

    public static bool DeleteAttribute(long attributeID)
    {
        return productDB.DeleteAttributes(attributeID);
    }
}

