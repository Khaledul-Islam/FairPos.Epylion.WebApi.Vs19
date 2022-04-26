using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
    public class BuyRequisition
    {
        public string CmpIDX { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime BuyDT { get; set; }
        public string sBarCode { get; set; }
        public string BarCode { get; set; }
        public string SKU { get; set; }
        public decimal CPU { get; set; }
        public decimal RPU { get; set; }
        public decimal WSP { get; set; }
        public decimal WSQ { get; set; }
        public decimal PrdComm { get; set; }
        public decimal DiscPrcnt { get; set; }
        public decimal VATPrcnt { get; set; }
        public decimal Qty { get; set; }
        public decimal sQty { get; set; }
        public decimal balQty { get; set; }
        public string SupID { get; set; }
        public DateTime EXPDT { get; set; }
        public string UserID { get; set; }
        public decimal Reorder { get; set; }
        public string ZoneID { get; set; }
        public decimal Point { get; set; }
        public string ApprovedBy { get; set; }
        public decimal ApproveQtY { get; set; }
        public string Transfer { get; set; }
        public string ShopID { get; set; }
        public string IsApproved { get; set; }
    }
}
