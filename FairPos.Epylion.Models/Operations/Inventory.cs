using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class Inventory
	{
		public decimal InventoryId { get; set; }
		public string InvenotryNo { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public decimal StockBoxQty { get; set; }
		public decimal WriteOffBoxQty { get; set; }
		public decimal WriteOnBoxQty { get; set; }
		public decimal BoxSize { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public decimal RPU { get; set; }
	}
}
