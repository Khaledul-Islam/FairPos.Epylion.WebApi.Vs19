using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class ConversionStock
	{
		public decimal ConversitionId { get; set; }
		public string ConversitionNo { get; set; }
		public DateTime ConDate { get; set; }
		public string FromSBarcode { get; set; }
		public string FromBarcodeExp { get; set; }
		public string ToSBarcode { get; set; }
		public string ToBarcodeExp { get; set; }
		public decimal FromBoxQty { get; set; }
		public decimal ToBoxQty { get; set; }
		public DateTime ExpireDate { get; set; }
		public decimal FromUnitQty { get; set; }
		public decimal ToUniQty { get; set; }
	}
}
