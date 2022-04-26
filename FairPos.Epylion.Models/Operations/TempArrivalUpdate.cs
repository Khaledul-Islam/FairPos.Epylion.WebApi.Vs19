using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class TempArrivalUpdate
	{
		public decimal TempArrivalId { get; set; }
		public string ARRIVAL_NO { get; set; }
		public string Chln { get; set; }
		public string SupID { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public string BarcodeExp { get; set; }
		public decimal POBoxQty { get; set; }
		public decimal PoQty { get; set; }
		public decimal BoxQty { get; set; }
		public decimal ArrivalQty { get; set; }
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
		public decimal PrdComPer { get; set; }
		public decimal PrdComAmnt { get; set; }
		public decimal AddCost { get; set; }
		public string ItemFullName { get; set; }
		public string BoxUOM { get; set; }
		public string UnitUOM { get; set; }
		public decimal PrvRcvQty { get; set; }
		public decimal PrvRcvBox { get; set; }
		public decimal BoxSize { get; set; }
		public string RefNo { get; set; }
		public DateTime RefDate { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordFilter { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordCount { get; set; }
	}
}
