using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
	public class StaffLedger
	{
		public decimal Id { get; set; }
		public string CollectionNo { get; set; }
		public string InvoiceNo { get; set; }
		public decimal EmpID { get; set; }
		public decimal DrAmount { get; set; }
		public decimal CrAmount { get; set; }
		public string Description { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

	}
}
