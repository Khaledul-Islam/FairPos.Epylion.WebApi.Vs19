using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class Buy
    {
        public string CMPIDX { get; set; }
        public string sBarCode { get; set; }
        public string BarCode { get; set; }
        public string Chln { get; set; }
        public DateTime BuyDT { get; set; }
        public decimal CPU { get; set; }
        public decimal LCPU { get; set; }
        public decimal RPU { get; set; }
        public decimal RPP { get; set; }
        public decimal WSP { get; set; }
        public decimal WSQ { get; set; }
        public decimal DiscPrcnt { get; set; }
        public decimal VATPrcnt { get; set; }
        public decimal PrdComm { get; set; }
        public decimal Qty { get; set; }
        public decimal CQty { get; set; }
        public decimal bQty { get; set; }
        public decimal TrfQty { get; set; }
        public decimal sreturn { get; set; }
        public decimal sQty { get; set; }
        public decimal rQty { get; set; }
        public decimal SrpQty { get; set; }
        public decimal dmlqty { get; set; }
        public decimal InvQty { get; set; }
        public decimal balQty { get; set; }
        public string SupID { get; set; }
        public DateTime EXPDT { get; set; }
        public DateTime LastSDT { get; set; }
        public string ShopID { get; set; }
        public string Transfer { get; set; }
        public string SupRef { get; set; }
        public string UserID { get; set; }
        public decimal Point { get; set; }
        public decimal Reorder { get; set; }
        public string ZoneID { get; set; }
        public DateTime BarocdeDate { get; set; }
        public decimal WriteOff { get; set; }
        public decimal WriteOn { get; set; }
        public decimal TransferIn { get; set; }
        public decimal TransferOut { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string ProductDescription { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string SupName { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string UOMName { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string PrdName { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string PrdID { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public bool AutoSale { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string BarCodeExp { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string ProductDescriptionBangla { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public decimal BoxSize { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string BoxUOMName { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string reason { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public bool IsDamageStock { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public bool IsConversionItem { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public decimal SaleBalQty { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public decimal ItemWeight { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public decimal reqbox { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public decimal reqqty { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string stocktype { get; set; }
        [FIK.DAL.FIK_NoCUD]

        public string CounterID { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal ItemName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public bool IsConverationItem { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal SpouseId { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal EmpID { get; set; }
    }

}
