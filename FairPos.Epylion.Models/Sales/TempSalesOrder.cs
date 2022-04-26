using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Sales
{
	public class TempSalesOrder
	{
		public decimal Temp_ID { get; set; }
		public decimal EmpID { get; set; }
		public string sBarcode { get; set; }
		public string ShopID { get; set; }
		public string Barcode { get; set; }
		public string ItemName { get; set; }
		public string SupID { get; set; }
		public decimal QTY { get; set; }
		public decimal CPU { get; set; }
		public decimal RPU { get; set; }
		public decimal VatPrcnt { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public DateTime EXPIRE_DATE { get; set; }
		public string CounterId { get; set; }
		public bool IsGift { get; set; }
		public string SalesStockType { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordFilter { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordCount { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public decimal TotalAmnt { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public decimal GrandTotal { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public decimal AvailableLimit { get; set; }

	}
}
