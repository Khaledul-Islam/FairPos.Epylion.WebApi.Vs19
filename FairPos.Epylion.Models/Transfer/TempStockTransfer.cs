using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Transfer
{
	public class TempStockTransfer
	{
		public decimal TempId { get; set; }
		public string UserId { get; set; }
		public string ShopId { get; set; }
		public string PrvCusID { get; set; }
		public string CustomerName { get; set; }
		public string CustomerMobile { get; set; }
		public string SBarocde { get; set; }
		public string Barcode { get; set; }
		public decimal TQty { get; set; }
		public decimal CPU { get; set; }
		public decimal RPU { get; set; }
		public string ProdcutDescription { get; set; }
		public string TransferFrom { get; set; }
		public string TransferTo { get; set; }
		public string Reason { get; set; }
		public bool IsDamageStock { get; set; }
		public string SupID { get; set; }
		public bool IsGiftItemAvailable { get; set; }
		public decimal BoxQty { get; set; }
		public string BoxUOM { get; set; }
		public string UnitUOM { get; set; }
		public DateTime EXPDT { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordFilter { get; set; }
		[FIK.DAL.FIK_NoCUD]
		public int RecordCount { get; set; }

	}
}
