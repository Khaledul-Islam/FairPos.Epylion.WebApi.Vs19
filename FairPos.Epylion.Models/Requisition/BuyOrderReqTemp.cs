using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Requisition
{
	public class BuyOrderReqTemp
	{
		public string CmpIDX { get; set; }
		public string Chln { get; set; }
		public string SupID { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public decimal BoxQty { get; set; }
		public decimal Qty { get; set; }
		public decimal sQty { get; set; }
		public decimal DiscPrcnt { get; set; }
		public decimal VATPrcnt { get; set; }
		public decimal PrdComm { get; set; }
		public decimal CPU { get; set; }
		public decimal RPU { get; set; }
		public decimal CRPU { get; set; }
		public DateTime BuyDT { get; set; }
		public DateTime EXPDT { get; set; }
		public string UserID { get; set; }
		public string PrdDescription { get; set; }
		public string BoxUOM { get; set; }
		public string UnitUOM { get; set; }
		public DateTime DeliveryDate { get; set; }
		public string PrdID { get; set; }
		public decimal POPackQty { get; set; }
		public string PackUOM { get; set; }
		public string ReqChlnNo { get; set; }
		public string ShopID { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public bool enabled { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public bool ischeck { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public bool PartialDelivery { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public string PaymentTerms { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public string MaturtyDays { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public string QutRefNo { get; set; }
	}
}
