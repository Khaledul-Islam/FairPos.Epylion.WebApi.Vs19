using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class TempPriceChanged
	{
		public decimal Tempid { get; set; }
		public string sBarcode { get; set; }
		public string Barcode { get; set; }
		public string ProductName { get; set; }
		public decimal RPU { get; set; }
		public decimal CngRPU { get; set; }
		public decimal EffectedbalQty { get; set; }
		public decimal CngAmount { get; set; }
		public decimal CngPrcnt { get; set; }
		public string Status { get; set; }
		public string UserId { get; set; }
	}
}
