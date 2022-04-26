using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class MemberCategory
    {
        public int MemCatId { get; set; }
        public string MemCatName { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }
    }
}
