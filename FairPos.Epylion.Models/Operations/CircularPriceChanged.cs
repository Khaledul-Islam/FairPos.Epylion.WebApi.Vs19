using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
    public class CircularPriceChanged
    {
        public int Id { get; set; }
        public string CPCNo { get; set; }
        public string CPCName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string IsActivated { get; set; }
        public bool IsApproved { get; set; }
    }

    public class CircularPriceChangedDetail
    {
        public int DetailsId { get; set; }
        public string CPCNo { get; set; }
        public string sBarcode { get; set; }
        public string Barcode { get; set; }
        public decimal RPU { get; set; }
        public decimal CngRPU { get; set; }
        public decimal EffectedbalQty { get; set; }
        public decimal CngAmount { get; set; }
        public decimal CngPrcnt { get; set; }
        public string Status { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string Description { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public DateTime? CreateDate { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string UserId { get; set; }
    }
}
