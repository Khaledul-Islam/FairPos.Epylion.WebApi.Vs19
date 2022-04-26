using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models
{
    public class Supplier
    {
        public string SupID { get; set; }

        public string RegName { get; set; }

        public string RegAdd1 { get; set; }

        public string RegAdd2 { get; set; }

        public string RegPhone { get; set; }

        public string RegFax { get; set; }

        public string RegWeb { get; set; }

        public string RegEmail { get; set; }

        public bool ChkRegOwner { get; set; }

        public bool ChkRegPartner { get; set; }

        public bool? chkCashCheq { get; set; }

        public bool? chkBEFTN { get; set; }

        public string Supname { get; set; }

        public string TradeAdd1 { get; set; }

        public string TradeAdd2 { get; set; }

        public string TradePhone { get; set; }

        public string TradeFax { get; set; }

        public string TradeWeb { get; set; }

        public string TradeEmail { get; set; }

        public bool chkTradeMember { get; set; }

        public bool chkTradeDirector { get; set; }

        public string GenCName { get; set; }

        public string GenCDesig { get; set; }

        public string GenCCell { get; set; }

        public string GenCEmail { get; set; }

        public string MgtCName { get; set; }

        public string MgtCDesig { get; set; }

        public string MgtCCell { get; set; }

        public string MgtCEmail { get; set; }

        public string MngCname { get; set; }

        public string MngCdesig { get; set; }

        public string MngCCell { get; set; }

        public string MngCEmail { get; set; }

        public string FinCname { get; set; }

        public string FinCDesig { get; set; }

        public string FinCCell { get; set; }

        public string FinCEmail { get; set; }

        public decimal? gMargin { get; set; }

        public decimal? gMarginTP { get; set; }

        public decimal? gMarginAVG { get; set; }

        public bool chkgMarginAP { get; set; }

        public decimal? asDays { get; set; }

        public decimal? sacDays { get; set; }

        public decimal? B2BDays { get; set; }

        public string SupType { get; set; }

        public decimal? crDays { get; set; }

        public bool chkAC { get; set; }

        public bool chkPO { get; set; }

        public bool chkCash { get; set; }

        public string IssueFor { get; set; }

        public string Bank { get; set; }

        public string BankBR { get; set; }

        public string BankBRCode { get; set; }

        public string ACCName { get; set; }

        public string ACCnum { get; set; }

        public string ACCNB { get; set; }

        public string ACCtype { get; set; }

        public string ACCMNB { get; set; }

        public bool chkSupDay { get; set; }

        public bool chkSupWeek { get; set; }

        public bool chkSupMonth { get; set; }

        public bool chkSupAsPer { get; set; }

        public decimal? DeliveryDays { get; set; }

        public string TransportMode { get; set; }

        public bool chkDamageRep { get; set; }

        public bool chkDamageRet { get; set; }

        public bool chkSlowRep { get; set; }

        public bool chkSlowRet { get; set; }

        public bool chkShortRep { get; set; }

        public bool chkShortRet { get; set; }

        public bool chkExpireRep { get; set; }

        public bool chkExpireRet { get; set; }

        public decimal? InformDay { get; set; }

        public bool chkTradeLicence { get; set; }

        public bool chkBSTIdocument { get; set; }

        public bool chkVATCertificate { get; set; }

        public bool chkTINCertificate { get; set; }

        public bool chkOtherDocument { get; set; }

        public bool chkTypeLocal { get; set; }

        public bool chkTypeForeign { get; set; }

        public bool chkTypeOther { get; set; }

        public string TraderEMOSCode { get; set; }

        public string SupArea { get; set; }

        public string SupCommodity { get; set; }

        public decimal? SPonTP { get; set; }

        public decimal? SPonMRP { get; set; }

        public decimal? SplDiscount { get; set; }

        public string SupCategory { get; set; }

        public bool? Fired { get; set; }

        public DateTime? DOE { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int? MaturityDays { get; set; }


        [FIK.DAL.FIK_NoCUD]
        public SupplierDoc supplierDoc { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }

    }
}
