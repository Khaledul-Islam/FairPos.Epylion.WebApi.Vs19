using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class DamageStock
	{
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public string Chln { get; set; }
		public DateTime BuyDT { get; set; }
		public decimal CPU { get; set; }
		public decimal LCPU { get; set; }
		public decimal RPU { get; set; }
		public decimal RPP { get; set; }
		public decimal WSP { get; set; }
		public decimal WSQ { get; set; }
		public decimal DiscPrcnt { get; set; }
		public decimal VATPrcnt { get; set; }
		public decimal PrdComm { get; set; }
		public decimal Qty { get; set; }
		public decimal SaleReturn { get; set; }
		public decimal rQty { get; set; }
		public decimal SrpQty { get; set; }
		public decimal dmlqty { get; set; }
		public decimal balQty { get; set; }
		public string SupID { get; set; }
		public DateTime EXPDT { get; set; }
		public string ShopID { get; set; }
		public string Transfer { get; set; }
		public string SupRef { get; set; }
		public string UserID { get; set; }
		public DateTime BarocdeDate { get; set; }
	}
}
