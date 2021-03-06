using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class MemberCategoryProduct
    {
        public int CatPrdId { get; set; }
        public int MemCatId { get; set; }
        public string PrdID { get; set; }
        public decimal LimitQty { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public bool Check { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string ProductName { get; set; }
    }
}
