using System;
using SV20T1020051.DomainModels;

namespace SV20T1020051.Web.Models
{
	public class OrderDetailModel
	{
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}

