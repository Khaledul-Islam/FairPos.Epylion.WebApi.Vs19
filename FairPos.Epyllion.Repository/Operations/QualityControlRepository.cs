using Dapper;
using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Operations
{
    public interface IQualityControlRepository
    {
        List<Supplier> QCSupplierDDL(string ShopID, string UserID);
        List<Arrival> GetPendingPONo(string supplierId, string ShopID, string UserID);
        List<TempQualityControl> GetTempQualityControl(string UserID, string ShopID);
        List<TempQualityControl> GetTempQualityControlByChln(string chln);
        List<StyleSize> GetChallanItems(string dcno);
        List<Arrival> GetChallanDetails(string dcno, string sBarcode);
        bool SaveTempArrival(List<Arrival> model, out string errMsg);
        bool DeleteTempQC(string UserID, out string errMsg);
        List<string> SaveQualityControl(List<TempQualityControl> model, out string errMsg);
    }

    public class QualityControlRepository : BaseRepository, IQualityControlRepository
    {
        public QualityControlRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }

        public bool DeleteTempQC(string UserID, out string errMsg)
        {
            errMsg = string.Empty;
            string sql = "DELETE FROM TempQualityControl WHERE UserID=@UserID ; SELECT @@ROWCOUNT";
            int row = _dapper.Query<int>(sql, new
            {
                @UserID = UserID
            }).FirstOrDefault();

            if (row == 0)
            {
                errMsg = "No item to delete";
                return false;
            }
            return true;
        }

        public List<Arrival> GetChallanDetails(string dcno, string sBarcode)
        {
            string query = "select * from Arrival  where ARRIVAL_NO='" + dcno + "' and sBarCode='" + sBarcode + "'";
            var response = _dal.Select<Arrival>(query, ref msg).ToList();
            return response;
        }

        public List<StyleSize> GetChallanItems(string dcno)
        {
            string query = "select a.sBarcode,vs.ItemFullName from Arrival a left join vStyleSize vs on a.sBarCode=vs.sBarcode where a.ARRIVAL_NO='" + dcno + "' and a.IS_QC=0;";
            var response = _dal.Select<StyleSize>(query, ref msg).ToList();
            return response;
        }

        public List<Arrival> GetPendingPONo(string supplierId, string ShopID, string UserID)
        {
            string query = "select distinct  ARRIVAL_NO from Arrival  where IS_QC=0  and SupID='" + supplierId + "' and UserID='" + UserID + "' and ShopID='" + ShopID + "'";
            var response = _dal.Select<Arrival>(query, ref msg).ToList();
            return response;
        }

        public List<TempQualityControl> GetTempQualityControl(string UserID, string ShopID)
        {
            string query = "select * from TempQualityControl where UserID = '" + UserID + "' and ShopID='" + ShopID + "'";
            var response = _dal.Select<TempQualityControl>(query, ref msg).ToList();
            return response;
        }

        public List<TempQualityControl> GetTempQualityControlByChln(string chln)
        {
            string query = "select * from TempQualityControl where chln = '" + chln + "'";
            var response = _dal.Select<TempQualityControl>(query, ref msg).ToList();
            return response;
        }

        public List<Supplier> QCSupplierDDL(string ShopID, string UserID)
        {
            string query = "select SupID,Supname from Supplier  where SupID in (SELECT DISTINCT SupID FROM Arrival   WHERE  IS_QC=0 and UserID='" + UserID + "' and ShopID='" + ShopID + "')";
            var response = _dal.Select<Supplier>(query, ref msg).ToList();
            return response;
        }

        public List<string> SaveQualityControl(List<TempQualityControl> details, out string errMsg)
        {
            errMsg = string.Empty;
            string year = DateTime.Now.Year.ToString() + "/";
            string QC_NO = GetMaxIdWithPrfix2("QC_NO", "0000", "0001", "QualityControl", "MRIR/" + year);
            string GP_NO = GetMaxIdWithPrfix2("GatePassNo", "0000", "0001", "GatePass", "GP/" + year);
            string deliveryChln = GetMaxIdWithPrfix2("DeliveryChlnNo", "0000", "0001", "QualityControl", "CN/" + year);
            string debit_no = GetMaxIdWithPrfix2("DebitNoteNo", "0000", "0001", "QualityControl", "DN/" + year);
            decimal total = details.FirstOrDefault().totalPrice;
            decimal cngCPU = 0;
            int max = details.Count;
            bool isFoundReturn = false;
            foreach (var d in details)
            {
                if (d.ArrivalQty > d.QCQty)
                {
                    isFoundReturn = true;
                }
                if (d.QCQty > d.ArrivalQty)
                {
                    errMsg = "QC Quantity cannot be greater then arival qty, Barcode no. " + d.BarCode;
                    return new List<string>() { "false","","" };
                }
            }
            if (isFoundReturn == false)
            {
                deliveryChln = "";
                debit_no = "";
            }
            List<QualityControl> lstqc = new();
            List<Buy> lstbuy = new();
            List<GatePass> lstgp = new();
            List<GiftStock> lstgift = new();
            List<StyleSize> lstStyle = new();

            foreach (var d in details)
            {
                #region Insert QualityControl
                QualityControl rc = new();
                rc.QC_NO = QC_NO;
                rc.ARRIVAL_NO = d.ARRIVAL_NO;
                rc.sBarCode = d.sBarCode;
                rc.BarCode = d.BarCode;
                rc.CPU = d.CPU;
                rc.RPU = d.RPU;
                rc.Chln = d.Chln;
                rc.ArrivalQty = d.ArrivalQty;
                rc.ArrivalBox = d.ArrivalBox;
                rc.QCQty = d.QCQty;
                rc.QCBoxQty = d.QCBoxQty;
                rc.GIFT_RATIO = d.GIFT_RATIO;
                rc.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                rc.BarcodeExp = d.BarcodeExp;
                rc.DiscPrcnt = d.DiscPrcnt;
                rc.VATPrcnt = d.VATPrcnt;
                rc.EXPDT = d.EXPDT;
                rc.BuyDT = DateTime.Now.Date;
                rc.ACPU = rc.CPU;
                rc.CPU = Math.Round(Convert.ToDecimal(rc.CPU), 2);
                rc.PrdComPer = 0;
                rc.PrdComAmnt = cngCPU;
                rc.AddCost = 0;
                rc.TotalPrdComm = 0;
                rc.SupID = d.SupID;
                rc.ReferenceNo = d.ReferenceNo;
                rc.UserID = d.UserID;
                rc.ShopID = d.ShopID;
                rc.DeliveryChlnNo = deliveryChln;
                rc.DebitNoteNo = debit_no;
                if (d.QCQty > 0)
                {
                    lstqc.Add(rc);//create QualityControl table
                }
                #endregion QualityControl

                #region buy

                string query = $@"select * from Buy  WHERE  sBarCode='{d.sBarCode}' and BarCode='{d.BarCode}' ";
                var dataPrv = _dal.Select<Buy>(query, ref msg).FirstOrDefault();


                Buy buy = new();
                buy.CMPIDX = QC_NO + d.sBarCode;
                buy.sBarCode = d.sBarCode;
                buy.BarCode = d.BarcodeExp;
                buy.Chln = QC_NO;
                buy.BuyDT = DateTime.Now;

                buy.CPU = rc.CPU;
                buy.RPU = d.RPU;

                if (dataPrv != null)
                {
                    buy.CPU = decimal.Round((((buy.CPU * buy.Qty) + (dataPrv.balQty * dataPrv.CPU)) / (buy.Qty + dataPrv.balQty)), 2);
                    buy.RPU = decimal.Round((((buy.RPU * buy.Qty) + (dataPrv.balQty * dataPrv.RPU)) / (buy.Qty + dataPrv.balQty)), 2);
                }

                buy.LCPU = rc.CPU;
                buy.RPP = 0;
                buy.CQty = 0;
                buy.WSP = 0;
                buy.WSQ = 0;
                buy.DiscPrcnt = d.DiscPrcnt;
                buy.VATPrcnt = d.VATPrcnt;
                buy.PrdComm = rc.PrdComAmnt;
                buy.Qty = rc.QCQty;
                buy.bQty = 0;
                buy.TrfQty = 0;
                buy.sreturn = 0;
                buy.sQty = 0;
                buy.rQty = 0;
                buy.SrpQty = 0;
                buy.dmlqty = 0;
                buy.BarocdeDate = d.EXPDT;
                buy.WriteOn = 0;
                buy.WriteOff = 0;
                buy.InvQty = 0;
                buy.balQty = d.QCQty;
                buy.SupID = d.SupID;
                buy.EXPDT = d.EXPDT;
                buy.ShopID = d.ShopID;
                buy.Transfer = "N";
                buy.SupRef = "NA";
                buy.UserID = d.UserID;
                buy.ShopID = d.ShopID;
                buy.Point = 0;
                buy.Reorder = 0;
                buy.ZoneID = "";
                if (rc.QCQty > 0)
                {
                    lstbuy.Add(buy);//create update buy table
                }

                lstStyle.Add(new StyleSize { sBarcode = d.sBarCode, Barcode = d.BarCode, CPU = buy.CPU, RPU = buy.RPU });

                #endregion buy
                #region GatePass
                if (d.ArrivalQty > d.QCQty)
                {
                    GatePass gp = new();
                    gp.AddCost = d.AddCost;
                    gp.ARRIVAL_NO = d.ARRIVAL_NO;
                    gp.BarCode = d.BarCode;
                    gp.BarcodeExp = d.BarcodeExp;
                    gp.Chln = d.Chln;
                    gp.CPU = d.AddCost;
                    gp.EXPDT = d.EXPDT;
                    gp.GatePassNo = GP_NO;
                    gp.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                    gp.GIFT_RATIO = d.GIFT_RATIO;
                    gp.GPDT = DateTime.Now.Date;
                    gp.GPQty = d.ArrivalBox - d.QCBoxQty;
                    gp.ArrivalQty = d.ArrivalBox;
                    gp.PrdComAmnt = d.AddCost;
                    gp.PrdComPer = d.AddCost;
                    gp.ReferenceNo = d.ReferenceNo;
                    gp.RPU = d.AddCost;
                    gp.sBarCode = d.sBarCode;
                    gp.SupID = d.SupID;
                    gp.UserID = d.UserID;
                    gp.ShopID = d.ShopID;
                    lstgp.Add(gp);//create gate pass
                }
                #endregion
                #region Create gift Stock
                if (d.GIFT_RATIO > 0)
                {
                    GiftStock gift = new();
                    gift.BalQty = d.QCBoxQty;
                    gift.Barcode = d.BarcodeExp;
                    gift.DmlQty = 0;
                    gift.Name = d.GIFT_DESCRIPTION;
                    gift.Qty = d.QCBoxQty;
                    gift.RtnQty = 0;
                    gift.sBarcode = d.sBarCode;
                    gift.SQty = 0;
                    gift.SRQty = 0;
                    gift.SupID = d.SupID;
                    gift.BarcodeDate = d.EXPDT;
                    gift.GIFT_RATIO = d.GIFT_RATIO;
                    gift.ShopID = d.ShopID;
                    lstgift.Add(gift);
                }
                #endregion gift stock

            }

            CompositeModel composite = new();
            if (lstqc.Count > 0)
            {
                composite.AddRecordSet<QualityControl>(lstqc, OperationMode.Insert, "QcId", "", "", "");
            }
            if (lstgift.Count > 0)
            {
                composite.AddRecordSet<GiftStock>(lstgift, OperationMode.Insert, "", "", "", "");
            }
            if (lstgp.Count > 0)
            {
                composite.AddRecordSet<GatePass>(lstgp, OperationMode.Insert, "GPId", "", "", "");
            }
            if (lstbuy.Count > 0)
            {
                string col = "CMPIDX,sBarCode,BarCode,Chln,BuyDT,CPU,LCPU,RPU,RPP,WSP,WSQ,DiscPrcnt,VATPrcnt," +
                               "PrdComm,+Qty,CQty,bQty,TrfQty,sreturn,sQty,rQty,SrpQty,dmlqty,InvQty,+balQty," +
                               "SupID,EXPDT,ShopID,Transfer,SupRef,UserID,Point,Reorder,ZoneID,BarocdeDate," +
                               "WriteOff,WriteOn,TransferIn,TransferOut ";
                composite.AddRecordSet<Buy>(lstbuy, OperationMode.InsertOrUpdaet, "", col, "BarCode,sBarCode", "");

                composite.AddRecordSet<StyleSize>(lstStyle, OperationMode.InsertOrUpdaet, "", "CPU,RPU", "BarCode,sBarCode", "");
            }

            //
            AccountsChln acc = new();
            acc.AddPrdComm = 0;
            acc.BuyDt = DateTime.Now;
            acc.Chln = QC_NO;
            acc.ChlnTotal = total;
            acc.SupID = details[0].SupID;
            acc.SupRef = details[0].ReferenceNo;
            acc.TotalPrdComm = 0;
            acc.Transfer = "N";
            acc.ShopID = details.First().ShopID;
            acc.UserID = details.FirstOrDefault().UserID;

            composite.AddRecordSet<AccountsChln>(acc, OperationMode.Insert, "", "", "", "");
            Arrival arrival = new() { ARRIVAL_NO = details.First().ARRIVAL_NO, sBarCode = details.First().sBarCode, IS_QC = true };
            composite.AddRecordSet<Arrival>(arrival, OperationMode.Update, "", "IS_QC", "ARRIVAL_NO,sBarCode", "");
            composite.AddRecordSet<TempQualityControl>(details.FirstOrDefault(), OperationMode.Delete, "", "", "UserID", "");
            bool response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return new List<string>() { "false","","" };
            }
            if (response)
            {
                return new List<string>() { QC_NO,deliveryChln,debit_no };
            }
            else
            {
                return new List<string>() { "false","","" };
            }

        }

        public bool SaveTempArrival(List<Arrival> model, out string errMsg)
        {
            errMsg = string.Empty;
            List<TempQualityControl> lstTempQC = new();
            foreach (var d in model)
            {
                var item = StyleSizeDetailsByBarcode(d.BarCode);
                //item.balQty = GetActualStockBySbarocde(item.sBarcode);
                TempQualityControl qc = new();
                qc.ArrivalQty = d.ArrivalQty;
                qc.ArrivalBox = d.BoxQty;
                qc.QCQty = d.ArrivalQty;
                qc.QCBoxQty = d.BoxQty;
                qc.BarCode = d.BarCode;
                qc.BarcodeExp = d.BarcodeExp;
                qc.ItemFullName = item.ItemName;
                qc.BuyDT = DateTime.Now;
                qc.Chln = d.Chln;//set
                qc.CPU = d.CPU;
                qc.DiscPrcnt = d.DiscPrcnt;
                qc.GIFT_RATIO = d.GIFT_RATIO;
                qc.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                qc.EXPDT = DateTime.Now.AddMonths(2);
                qc.PrdComm = d.PrdComm;
                qc.RPU = d.RPU;
                qc.sBarCode = d.sBarCode;
                qc.SupID = d.SupID;
                qc.UserID = d.UserID;//set
                qc.VATPrcnt = d.VATPrcnt;
                qc.PrdComPer = d.PrdComPer;
                qc.PrdComAmnt = d.PrdComAmnt;
                qc.AddCost = d.AddCost;
                qc.ARRIVAL_NO = d.ARRIVAL_NO;
                qc.Chln = d.Chln;
                qc.ReferenceNo = d.ReferenceNo;
                qc.UnitUOM = item.UOMName;
                qc.BoxUOM = item.BOXUOMName;
                qc.ShopID = d.ShopID;
                lstTempQC.Add(qc);
            }
            bool response = _dal.Insert<TempQualityControl>(lstTempQC, "", "QcId", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }
        private decimal GetActualStockBySbarocde(string sBarcode)
        {
            decimal sum = 0;
            string query = "select sum(balQty) as balQty from buy where sBarCode='" + sBarcode + "'";
            var response = _dal.SelectFirstColumn(query, ref msg);
            decimal.TryParse(response, out sum);
            return sum;
        }
        private StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            var query = "select * from vStyleSize where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }
    }
}
