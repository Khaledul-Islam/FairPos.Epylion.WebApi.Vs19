﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class TempInventory
	{
		public decimal TempId { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public decimal StockBoxQty { get; set; }
		public decimal WriteOffBoxQty { get; set; }
		public decimal WriteOnBoxQty { get; set; }
		public string UnitUOM { get; set; }
		public decimal BoxSize { get; set; }
		public string ProductDescription { get; set; }
		public string CREATE_BY { get; set; }
		public decimal RPU { get; set; }
		public decimal NewStockQty { get; set; }
		[FIK.DAL.FIK_NoCUD]
        public string UserID { get; set; }
		[FIK.DAL.FIK_NoCUD]
        public string ShopId { get; set; }
		[FIK.DAL.FIK_NoCUD]
        public decimal stock { get; set; }
    }
}
