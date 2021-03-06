using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class DebitNote
	{
		public decimal DNId { get; set; }
		public string DN_NO { get; set; }
		public string Chln { get; set; }
		public string SupID { get; set; }
		public string ShopID { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public string BarcodeExp { get; set; }
		public decimal POBoxQty { get; set; }
		public decimal PoQty { get; set; }
		public decimal ShortBoxQty { get; set; }
		public decimal ShortArrivalQty { get; set; }
		public decimal DiscPrcnt { get; set; }
		public decimal VATPrcnt { get; set; }
		public decimal PrdComm { get; set; }
		public decimal CPU { get; set; }
		public decimal RPU { get; set; }
		public DateTime BuyDT { get; set; }
		public DateTime EXPDT { get; set; }
		public string UserID { get; set; }
		public decimal GIFT_RATIO { get; set; }
		public string GIFT_DESCRIPTION { get; set; }
		public string ARRIVAL_NO { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordFilter { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordCount { get; set; }
	}
}
