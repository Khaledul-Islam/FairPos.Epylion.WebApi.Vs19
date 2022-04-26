using Dapper;
using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Requisition;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Requisition
{
    public interface IRequisitionToPoRepository
    {
        List<AutoRequistion> GetNonPoRequesitionNo();
        List<BuyOrderReqTemp> GetTempDataByUser(string userId);
        List<AutoRequistion> GetChallanDetails(string chlnNo);
        IEnumerable<BuyOrderReqTemp> GetsBuyOrderReqTemp(string chln);
        bool LoadRequisition(string UserID, string shopID, string chlnn, out string errMsg);
        bool LoadRequisitionAll(string UserID, string shopID, string month, string supID, out string errMsg);
        bool RemoveTmpRequisition(string UserID);
        bool UpdateRPUDeliveryDate(string barcode, string userid, decimal rpu, DateTime delDate);

        string SaveRequisitionToPO(List<BuyOrderReqTemp> olist, out string errMsg);
    }

    public class RequisitionToPoRepository : BaseRepository, IRequisitionToPoRepository
    {
        public RequisitionToPoRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }

        public List<AutoRequistion> GetChallanDetails(string chlnNo)
        {
            string query = "select * from AutoRequistion where Chln='" + chlnNo + "'";
            var response = _dal.Select<AutoRequistion>(query, ref msg).ToList();
            if (response == null || !string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }
        private List<AutoRequistion> GetChallanDetailsAll(string month, string supID, string ShopID, string UserID)
        {
            string query = "select * from AutoRequistion where UserID='" + UserID + "' AND  ShopID='" + ShopID + "' AND DATEPART(MONTH, ApprovedDate)='" + month + "' AND (SupID='" + supID + "' OR '" + supID + "' = '')";
            var response = _dal.Select<AutoRequistion>(query, ref msg).ToList();
            if (response == null || !string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }

        public List<AutoRequistion> GetNonPoRequesitionNo()
        {
            string query = @"SELECT DISTINCT Chln 
                            FROM dbo.AutoRequistion
                            WHERE ApproveStatus='Approved' AND IsPODone=0
                            AND Chln NOT IN (SELECT ReqChlnNo FROM BuyOrderReqTemp )";
            var response = _dapper.Query<AutoRequistion>(query).ToList();
            if (response == null)
            {
                return null;
            }
            return response;

        }
        private List<AutoRequistion> GetNonPoRequesitionNoALL(string month)
        {
            string query = "SELECT * FROM dbo.AutoRequistion WHERE DATEPART(MONTH, ApprovedDate)='" + month + "' AND IsPODone='0' AND ApproveStatus='Approved' AND IsPODone=0 AND Chln NOT IN (SELECT ReqChlnNo FROM BuyOrderReqTemp )";
            var response = _dapper.Query<AutoRequistion>(query).ToList();
            if (response == null)
            {
                return null;
            }
            return response;

        }

        public IEnumerable<BuyOrderReqTemp> GetsBuyOrderReqTemp(string chln)
        {
            string query = "select * from BuyOrderReqTemp where Chln='" + chln + "'";
            var response = _dal.Select<BuyOrderReqTemp>(query, ref msg).ToList();
            if (response == null || !string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }

        public List<BuyOrderReqTemp> GetTempDataByUser(string userId)
        {
            string query = "select * from BuyOrderReqTemp where UserID='" + userId + "'";
            var response = _dal.Select<BuyOrderReqTemp>(query, ref msg).ToList();
            if (response == null || !string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }

        public bool LoadRequisition(string UserID, string shopID, string chlnn, out string errMsg)
        {
            errMsg = string.Empty;

            List<AutoRequistion> requistion = _dal.Select<AutoRequistion>("select * from AutoRequistion where Chln='"+chlnn+"' and IsPODone='0'", ref msg).ToList();
            if (requistion == null || requistion.Count == 0)
            {
                errMsg = "No requisition found";
                return false;
            }
            List<BuyOrderReqTemp> requistionTemp = GetsBuyOrderReqTemp(null).ToList();
            CompositeModel composite = new();

            if (requistionTemp.Count > 0)
            {
                errMsg = "A PO is already processing by another user";
                return false;
            }
            string chln = GetMaxIdWithPrfix("RequisitionNo", "000000", "000001", "BuyRequisition", shopID);
            string cols = @"BarCode,RPU,CRPU,CPU,CmpIDX,sBarCode,+Qty,+BoxQty,+POPackQty,UserID,PrdDescription,BuyDT,EXPDT,BoxUOM,UnitUOM,
                            PackUOM,DeliveryDate,Chln,PrdComm,VATPrcnt,DiscPrcnt,sQty,ReqChlnNo,SupID";
            foreach (AutoRequistion d in requistion)
            {
                if (d.ApprovedBoxQty <= 0)
                {
                    continue;
                }
                BuyOrderReqTemp otemp = new();
                otemp.BarCode = d.BarCode;
                otemp.RPU = d.RPU;
                otemp.CRPU = d.RPU;
                otemp.CPU = d.CPU;
                otemp.CmpIDX = chln + d.BarCode;
                otemp.sBarCode = d.sBarCode;
                otemp.Qty = d.ApprovedBoxQty * d.BoxSize;
                otemp.BoxQty = d.ApprovedBoxQty;
                otemp.POPackQty = d.ApprovedBoxQty / d.POPackSize;
                otemp.UserID = UserID;
                otemp.PrdDescription = d.PrdDescription;
                otemp.BuyDT = DateTime.Now.Date;
                otemp.EXPDT = DateTime.Now.Date;
                otemp.BoxUOM = d.BoxUOM;
                otemp.UnitUOM = d.UnitUOM;
                otemp.PackUOM = d.PackUOM;
                otemp.DeliveryDate = DateTime.Now;
                otemp.Chln = d.Chln;
                otemp.PrdComm = 0;
                otemp.VATPrcnt = 0;
                otemp.DiscPrcnt = 0;
                otemp.sQty = 0;
                otemp.ReqChlnNo = d.Chln;
                otemp.SupID = d.SupID;
                composite.AddRecordSet<BuyOrderReqTemp>(otemp, OperationMode.InsertOrUpdaet, "", cols, "UserID,sBarCode,DeliveryDate", "");
            }
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool LoadRequisitionAll(string UserID, string shopID, string month, string supID, out string errMsg)
        {
            errMsg = string.Empty;
            var data = GetNonPoRequesitionNoALL(month).ToList();
            if (data == null || data.Count == 0)
            {
                errMsg = "No requisition found";
                return false;
            }
            List<AutoRequistion> requistion = GetChallanDetailsAll(month, supID, shopID, UserID);
            if (requistion == null || requistion.Count == 0)
            {
                errMsg = "No requisition found";
                return false;
            }
            List<BuyOrderReqTemp> requistionTemp = GetsBuyOrderReqTemp(null).ToList();
            CompositeModel composite = new();

            if (requistionTemp.Count > 0)
            {
                errMsg = "A PO is already processing by another user";
                return false;
            }
            string chln = GetMaxIdWithPrfix("RequisitionNo", "000000", "000001", "BuyRequisition", shopID);
            string cols = @"BarCode,RPU,CRPU,CPU,CmpIDX,sBarCode,+Qty,+BoxQty,+POPackQty,UserID,PrdDescription,BuyDT,EXPDT,BoxUOM,UnitUOM,
                            PackUOM,DeliveryDate,Chln,PrdComm,VATPrcnt,DiscPrcnt,sQty,ReqChlnNo,SupID,ShopID";
            foreach (AutoRequistion d in requistion)
            {
                if (d.ApprovedBoxQty <= 0)
                {
                    continue;
                }
                BuyOrderReqTemp otemp = new();
                otemp.BarCode = d.BarCode;
                otemp.RPU = d.RPU;
                otemp.CRPU = d.RPU;
                otemp.CPU = d.CPU;
                otemp.CmpIDX = chln + d.BarCode;
                otemp.sBarCode = d.sBarCode;
                otemp.Qty = d.ApprovedBoxQty * d.BoxSize;
                otemp.BoxQty = d.ApprovedBoxQty;
                otemp.POPackQty = d.ApprovedBoxQty / d.POPackSize;
                otemp.UserID = UserID;
                otemp.PrdDescription = d.PrdDescription;
                otemp.BuyDT = DateTime.Now.Date;
                otemp.EXPDT = DateTime.Now.Date;
                otemp.BoxUOM = d.BoxUOM;
                otemp.UnitUOM = d.UnitUOM;
                otemp.PackUOM = d.PackUOM;
                otemp.DeliveryDate = DateTime.Now;
                otemp.Chln = d.Chln;
                otemp.PrdComm = 0;
                otemp.VATPrcnt = 0;
                otemp.DiscPrcnt = 0;
                otemp.sQty = 0;
                otemp.ReqChlnNo = d.Chln;
                otemp.SupID = d.SupID;
                otemp.ShopID = d.ShopID;
                composite.AddRecordSet<BuyOrderReqTemp>(otemp, OperationMode.InsertOrUpdaet, "", cols, "UserID,sBarCode,DeliveryDate", "");
            }
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool RemoveTmpRequisition(string UserID)
        {
            string query = "delete from BuyOrderReqTemp where UserID = '" + UserID + "' ;select @@ROWCOUNT";
            var response = _dapper.Query<int>(query).FirstOrDefault();
            if (response == 0)
            {
                return false;
            }
            return true;

        }

        public bool UpdateRPUDeliveryDate(string barcode, string userid, decimal rpu, DateTime delDate)
        {
            BuyOrderReqTemp data = new();
            data.CRPU = rpu;
            data.DeliveryDate = delDate;
            data.UserID = userid;
            data.BarCode = barcode;
            var response = _dal.Update<BuyOrderReqTemp>(data, "CRPU,DeliveryDate", "UserID,BarCode", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                return false;
            }
            return response;
        }

        public string SaveRequisitionToPO(List<BuyOrderReqTemp> lstBuyTemp, out string errMsg)
        {

            try
            {
                errMsg = string.Empty;
                string Tempchln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "BuyOrderPriceApproval", "Temp/" + DateTime.Now.Year.ToString() + "/");
                string chln = GetMaxIdWithPrfix("Chln", "0000", "0001", "BuyOrder", "PO/" + DateTime.Now.Year.ToString() + "/");
                bool isPriceChanged = false;
                CompositeModel composite = new();

                List<BuyOrderReqTemp> olist = GetTempDataByUser(lstBuyTemp.First().UserID);

                foreach (var item in lstBuyTemp)
                {
                    string barcode = item.BarCode;
                    BuyOrderReqTemp temp = olist.Find(m => m.BarCode == barcode);
                    temp.CRPU = item.CRPU;
                    temp.CPU = item.CPU;


                    if (temp.CRPU != item.CPU)
                    {
                        isPriceChanged = true;
                    }

                }
                if (isPriceChanged == false)
                {

                    foreach (var d in lstBuyTemp)
                    {
                        BuyOrder sts = new();
                        sts.BarCode = d.BarCode;
                        sts.sBarCode = d.sBarCode;
                        sts.Chln = chln;
                        sts.CmpIDX = sts.Chln + sts.BarCode + d.DeliveryDate.ToShortDateString();
                        sts.CPU = d.CPU;
                        sts.Qty = d.Qty;
                        sts.BoxQty = d.BoxQty;
                        sts.POPackQty = d.POPackQty;
                        sts.BuyDT = d.BuyDT;
                        sts.EXPDT = d.DeliveryDate; //dtpDeliveryDate.Value.Date;
                        sts.RPU = d.CRPU;
                        sts.SupID = d.SupID;
                        sts.PrdComm = d.PrdComm;
                        sts.DiscPrcnt = d.DiscPrcnt;
                        sts.VATPrcnt = 0;
                        sts.sQty = d.sQty;
                        sts.SupID = d.SupID;
                        sts.UserID = d.UserID;
                        sts.IS_ARRIVAL = false;
                        sts.IS_CANCEL = false;
                        sts.VatAmt = 0;
                        sts.QutRefNo = d.QutRefNo;
                        sts.PartialDelivery = d.PartialDelivery;
                        sts.MaturtyDays = d.MaturtyDays;
                        sts.PaymentTerms = d.PaymentTerms;
                        sts.ReqChlnNo = d.Chln;

                        composite.AddRecordSet<BuyOrder>(sts, OperationMode.Insert, "", "", "", "");
                        AutoRequistion req = new();
                        req.Chln = d.Chln;
                        req.BarCode = d.BarCode;
                        req.IsPODone = true;
                        composite.AddRecordSet<AutoRequistion>(req, OperationMode.Update, "", "IsPODone", "BarCode,Chln", "");
                    }
                    BuyOrderReqTemp reqtmp = new();
                    reqtmp.UserID = lstBuyTemp.First().UserID;
                    composite.AddRecordSet<BuyOrderReqTemp>(reqtmp, OperationMode.Delete, "", "", "UserID", "");

                    var res = _dal.InsertUpdateComposite(composite, ref msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg = msg;
                        return "false";
                    }
                    return chln;
                }
                else
                {

                    foreach (var d in lstBuyTemp)
                    {
                        BuyOrderPriceApproval sts = new();
                        sts.BarCode = d.BarCode;
                        sts.sBarCode = d.sBarCode;
                        sts.Chln = Tempchln;
                        sts.CmpIDX = sts.Chln + sts.BarCode + d.DeliveryDate.ToShortDateString();
                        sts.CPU = d.CPU;
                        sts.Qty = d.Qty;
                        sts.BoxQty = d.BoxQty;
                        sts.POPackQty = d.POPackQty;

                        sts.BuyDT = d.BuyDT;
                        sts.EXPDT = d.DeliveryDate; //dtpDeliveryDate.Value.Date;
                        sts.RPU = d.CRPU;
                        sts.OldRPU = d.RPU;
                        sts.SupID = d.SupID;

                        sts.PrdComm = d.PrdComm;
                        sts.DiscPrcnt = d.DiscPrcnt;
                        sts.VATPrcnt = 0;
                        sts.sQty = d.sQty;
                        sts.SupID = d.SupID;
                        sts.UserID = d.UserID;

                        sts.IS_ARRIVAL = false;
                        sts.IS_CANCEL = false;

                        sts.VatAmt = 0;
                        sts.QutRefNo = d.QutRefNo;
                        sts.PartialDelivery = d.PartialDelivery;
                        sts.MaturtyDays = d.MaturtyDays;
                        sts.PaymentTerms = d.PaymentTerms;
                        sts.ReqChlnNo = d.Chln;
                        sts.IsApproved = false;

                        composite.AddRecordSet<BuyOrderPriceApproval>(sts, OperationMode.Insert, "", "", "", "");
                        AutoRequistion req = new();
                        req.Chln = d.Chln;
                        req.BarCode = d.BarCode;
                        req.IsPODone = true;
                        composite.AddRecordSet<AutoRequistion>(req, OperationMode.Update, "", "IsPODone", "BarCode,Chln", "");
                    }

                    BuyOrderReqTemp reqtmp = new();
                    reqtmp.UserID = lstBuyTemp.First().UserID;
                    composite.AddRecordSet<BuyOrderReqTemp>(reqtmp, OperationMode.Delete, "", "", "UserID", "");

                    var res = _dal.InsertUpdateComposite(composite, ref msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        errMsg = msg;
                        return "false";
                    }
                    return chln;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
