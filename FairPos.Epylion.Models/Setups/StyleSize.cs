﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class StyleSize
    {
        public string CMPIDX { get; set; }
        public string sBarcode { get; set; }
        public string Barcode { get; set; }
        public string PrdID { get; set; }
        public string ItemName { get; set; }
        public string ItemNameBangla { get; set; }
        public decimal DiscPrcnt { get; set; }
        public decimal VATPrcnt { get; set; }
        public decimal PrdComm { get; set; }
        public decimal CPU { get; set; }
        public decimal RPU { get; set; }
        public decimal RPP { get; set; }
        public decimal WSP { get; set; }
        public decimal WSQ { get; set; }
        public bool DisContinued { get; set; }
        public string SupID { get; set; }
        public string SupName { get; set; }
        public string UserID { get; set; }
        public DateTime ENTRYDT { get; set; }
        public string ZoneID { get; set; }
        public decimal Point { get; set; }
        public decimal Reorder { get; set; }
        public decimal MaxOrder { get; set; }
        public int UOMId { get; set; }
        public int ExpireLimit { get; set; }
        public bool AutoSale { get; set; }
        public bool isTrail { get; set; }
        public DateTime? trailFrom { get; set; }
        public DateTime? trailTo { get; set; }
        public decimal BoxSize { get; set; }
        public int BoxUOMId { get; set; }
        public bool IsConverationItem { get; set; }
        public decimal MinOrder { get; set; }
        public int? ArrivalExpireLimit { get; set; }
        public decimal POPackSize { get; set; }
        public int PackUOMId { get; set; }
        public bool isAuthorized{ get; set; }
        public decimal ItemWeight { get; set; }
        public bool IsEssentialItem { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public string PrdName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string UOMName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string PackUiomName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string BOXUOMName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string balQty { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public string ItemFullName { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public List<StyleSize> itemsConvert { get; set; }
        public List<StyleSize> itemsList { get; set; }
    }
}
