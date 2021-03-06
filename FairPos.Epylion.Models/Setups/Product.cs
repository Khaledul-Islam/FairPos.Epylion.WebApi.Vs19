using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models
{
    public class Product
    {
        public string PrdID { get; set; }

        public string PrdName { get; set; }

        public string FloorID { get; set; }

        public decimal? VATPrcnt { get; set; }

        public decimal? DiscPrcnt { get; set; }

        public string PrdNameBangla { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; }     

        public DateTime? UpdateDate { get; set; }



        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public decimal LimitQty { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public bool cbCheck { get; set; }


    }
}
