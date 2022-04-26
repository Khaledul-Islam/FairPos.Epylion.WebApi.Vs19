using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Sales
{
	public class Sale
	{
		public DateTime SaleDT { get; set; }
		public string CmpIDX { get; set; }
		public string Invoice { get; set; }
		public string DelveryNo { get; set; }
		public string SupID { get; set; }
		public decimal Qty { get; set; }
		public decimal CPU { get; set; }
		public decimal RPP { get; set; }
		public decimal RPU { get; set; }
		public decimal VPP { get; set; }
		public decimal VPU { get; set; }
		public decimal WSP { get; set; }
		public decimal WSQ { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public decimal SQty { get; set; }
		public decimal rQty { get; set; }
		public decimal rAmt { get; set; }
		public decimal rVat { get; set; }
		public decimal rDisc { get; set; }
		public string cInvoice { get; set; }
		public string ShopID { get; set; }
		public string PayType { get; set; }
		public decimal TotalCost { get; set; }
		public decimal TotalAmt { get; set; }
		public decimal DiscPrcnt { get; set; }
		public decimal DiscAmtPrd { get; set; }
		public decimal DiscAmt { get; set; }
		public decimal VAT { get; set; }
		public decimal VATPrd { get; set; }
		public decimal NetAmt { get; set; }
		public string PrdSlNo { get; set; }
		public decimal CshAmt { get; set; }
		public decimal CrdAmt { get; set; }
		public string Salesman { get; set; }
		public string CardName { get; set; }
		public string CardNo { get; set; }
		public string CounterID { get; set; }
		public string PrvCus { get; set; }
		public string PrvCusID { get; set; }
		public string GrandID { get; set; }
		public string ParentID { get; set; }
		public string CusName { get; set; }
		public string Posted { get; set; }
		public decimal VATPrcnt { get; set; }
		public string Returned { get; set; }
		public string ReturnedType { get; set; }
		public DateTime ReturnedDt { get; set; }
		public string Flag { get; set; }
		public decimal Point { get; set; }
		public string IsCircularDiscount { get; set; }
		public string PackageNo { get; set; }
		public string UserId { get; set; }
		public decimal EmpID { get; set; }
		public bool IsGift { get; set; }
		public string SalesStockType { get; set; }
	}
}
