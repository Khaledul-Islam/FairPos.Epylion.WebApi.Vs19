using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class SupportBuy
    {
        public decimal BalQty { get; set; }
        public string Barcode { get; set; }
        public string sBarcode { get; set; }
        public string PrdID { get; set; }
        public string PrdName { get; set; }
        public string SupID { get; set; }
        public string ShopID { get; set; }
        public string Supname { get; set; }
        public string UOMName { get; set; }
        public string BoxUOMName { get; set; }
        public string ItemName { get; set; }
        public decimal ItemWeight{ get; set; }
        public string ItemFullName { get; set; }
        public string ItemNameBangla { get; set; }
        public string ItemFullNameBangla { get; set; }
        public decimal RPU { get; set; }
        public decimal CPU { get; set; }
        public decimal BoxSize { get; set; }
        public decimal VATPrcnt { get; set; }
        public decimal SO_Pending { get; set; }
        public decimal AppliedQty { get; set; }
        public bool AutoSale { get; set; }
        public bool IsConverationItem { get; set; }
        public DateTime EXPDT { get; set; }
    }
}
