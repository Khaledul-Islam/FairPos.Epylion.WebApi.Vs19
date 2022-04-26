using Dapper;
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
    public interface IRequisitionApprovalRepository
    {
        List<AutoRequistion> GetNonApprovedRequistion(string status, string shopid, out string errMsg);
        AutoRequistionTempApproval GetBychln(string id, out string errMsg);
        List<AutoRequistion> AutoRequistionsApprovalLoad(string shopid, out string msgg);
        bool SaveApproveRequisition(List<AutoRequistion> olist, out string errMsg);
        bool RequisitionReject(List<AutoRequistion> olist, out string errMsg);
        //req  to po approval section
        List<string> TempPODDL();
        List<BuyOrderPriceApproval> GetChallanDetails(string tempPoNo);
        string saveBuyOrderPOApproval(List<BuyOrderPriceApproval> lstbuy, out string errMsg);

    }

    public class RequisitionApprovalRepository : BaseRepository, IRequisitionApprovalRepository
    {
        public RequisitionApprovalRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }

        public List<AutoRequistion> AutoRequistionsApprovalLoad(string shopid, out string msgg)
        {
            msgg = string.Empty;
            try
            {
                var olist = GetNonApprovedRequistion("NEW", shopid, out string errMsg);
                if (olist == null || olist.Count == 0)
                {
                    msgg = "No Requisition Found";
                    return null;
                }
                string UserID = olist.First().UserID;
                string chln = "";
                foreach (var d in olist)
                {
                    d.ApprovedBoxQty = d.ActualBoxQty;

                    var temp = GetBychln(d.Chln, out string error);
                    if (temp != null)
                    {
                        if (temp.UserId != d.UserID)
                        {
                            msgg = "Another user processing this record";
                            return null;
                        }
                    }
                    else
                    {
                        chln = d.Chln;
                    }
                }
                if (chln != "")
                {
                    AutoRequistionTempApproval temp2 = new();
                    temp2.UserId = UserID;
                    temp2.CreateDate = DateTime.Now;
                    temp2.Chln = chln;
                    var response = _dal.Insert<AutoRequistionTempApproval>(temp2, "", "", "", ref msg);
                    if (response == false || !string.IsNullOrEmpty(msg))
                    {
                        msgg = "Response false." + msg;
                        return null;
                    }
                }
                return olist;
            }
            catch (Exception e)
            {
                msgg = e.Message;
                throw;
            }

        }

        public AutoRequistionTempApproval GetBychln(string id, out string errMsg)
        {
            errMsg = string.Empty;
            string query = "select * from AutoRequistionTempApproval where Chln='" + id + "'";
            var response = _dal.Select<AutoRequistionTempApproval>(query, ref msg).FirstOrDefault();
            if (response == null)
            {
                errMsg = "No record found";
                return null;
            }
            return response;
        }

        public List<AutoRequistion> GetNonApprovedRequistion(string status, string shopid, out string errMsg)
        {
            errMsg = string.Empty;
            string query = "select * from AutoRequistion where ApproveStatus='" + status + "' AND ShopID='" + shopid + "'";
            var response = _dal.Select<AutoRequistion>(query, ref msg).ToList();
            if (response == null)
            {
                errMsg = "No record found";
                return null;
            }
            return response;
        }

        public bool RequisitionReject(List<AutoRequistion> olist, out string errMsg)
        {
            errMsg = string.Empty;
            if (olist == null || olist.Count == 0)
            {
                errMsg = "No Requisition Found";
                return false;
            }
            CompositeModel composite = new();
            List<AutoRequistionTemp> lsttmp = new();
            foreach (var d in olist)
            {
                AutoRequistionTemp req = new();
                req.ActualBoxQty = d.ActualBoxQty;
                req.BalQty = d.BalQty;
                req.BarCode = d.BarCode;
                req.BoxQty = d.BoxQty;
                req.BoxSize = d.BoxSize;
                req.BoxUOM = d.BoxUOM;
                req.BuyDT = DateTime.Now;
                req.Chln = "c";
                req.CmpIDX = "c" + d.BarCode;
                req.CPU = d.CPU;
                req.DiscPrcnt = d.DiscPrcnt;
                req.MinOrder = d.MinOrder;
                req.PackUOM = d.PackUOM;
                req.POPackQty = d.POPackQty;
                req.POPackSize = d.POPackSize;
                req.PrdComm = d.PrdComm;
                req.PrdDescription = d.PrdDescription;
                req.PrdID = d.PrdID;
                req.Qty = d.Qty;
                req.RPU = d.RPU;
                req.sBarCode = d.sBarCode;
                req.sQty = d.sQty;
                req.SupID = d.SupID;
                req.UnitUOM = d.UnitUOM;
                req.UserID = d.UserID;
                req.VATPrcnt = d.VATPrcnt;
                req.Remarks = d.Remarks;
                req.ShopID = d.ShopID;
                lsttmp.Add(req);
            }
            composite.AddRecordSet<AutoRequistionTemp>(lsttmp, OperationMode.Insert, "", "", "", "");
            composite.AddRecordSet<AutoRequistion>(olist, OperationMode.Update, "", "ApproveStatus,ApprovedDate,ApprovedBy", "Chln", "");
            AutoRequistionTempApproval arta = new();
            arta.UserId = olist.First().UserID;
            composite.AddRecordSet<AutoRequistionTempApproval>(arta, OperationMode.Delete, "", "", "UserId", "");
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool SaveApproveRequisition(List<AutoRequistion> olist, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            composite.AddRecordSet<AutoRequistion>(olist, OperationMode.Update, "", "ApprovedBoxQty,ApprovedBy,ApprovedDate,ApproveStatus", "BarCode,Chln", "");
            composite.AddRecordSet<AutoRequistionTempApproval>(olist, OperationMode.Delete, "", "", "UserId,ShopID", "");
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public List<string> TempPODDL()
        {
            string query = @"SELECT DISTINCT Chln
	                        FROM BuyOrderPriceApproval
	                        WHERE  IsApproved=0 ";
            return _dapper.Query<string>(query).ToList();
        }
        public List<BuyOrderPriceApproval> GetChallanDetails(string tempPoNo)
        {
            string query = @"sp_getBuyOrderPriceApprovalDetails '" + tempPoNo + "' ";
            var response = _dal.Select<BuyOrderPriceApproval>(query, ref msg).ToList();

            List<BuyOrderPriceApproval> olist = new();

            foreach (var result in response)
            {
                BuyOrderPriceApproval ostyle = new();
                ostyle.VATPrcnt = result.VATPrcnt;
                ostyle.UserID = result.UserID;
                ostyle.SupID = result.SupID;
                ostyle.sQty = result.sQty;
                ostyle.sBarCode = result.sBarCode;
                ostyle.RPU = result.RPU;
                ostyle.Qty = result.Qty;
                ostyle.PrdComm = result.PrdComm;
                ostyle.ItemFullName = result.ItemFullName;
                ostyle.EXPDT = result.EXPDT;
                ostyle.DiscPrcnt = result.DiscPrcnt;
                ostyle.CPU = result.CPU;
                ostyle.Chln = result.Chln;
                ostyle.BuyDT = result.BuyDT;
                ostyle.BarCode = result.BarCode;
                ostyle.VatAmt = result.VatAmt;
                ostyle.BoxQty = result.BoxQty;
                ostyle.POPackQty = result.POPackQty;
                ostyle.OldRPU = result.OldRPU;
                ostyle.UOMName = result.UOMName;
                ostyle.BoxUOMName = result.BoxUOMName;
                ostyle.PackUOM = result.PackUiomName;

                ostyle.IsApproved = result.IsApproved;
                olist.Add(ostyle);
            }

            return olist;
        }

        public string saveBuyOrderPOApproval(List<BuyOrderPriceApproval> lstbuy, out string errMsg)
        {
            errMsg = string.Empty;
            string chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "BuyOrder", "PO/" + DateTime.Now.Year.ToString() + "/");

            if (lstbuy.Count > 0)
            {
                if (lstbuy[0].IsApproved == true)
                {
                    errMsg = "This PO already approved";
                    return "false";
                }
            }
            CompositeModel composite = new();
            foreach (BuyOrderPriceApproval d in lstbuy)
            {
                BuyOrder sts = new();
                sts.BarCode = d.BarCode;
                sts.sBarCode = d.sBarCode;
                sts.Chln = chln;
                sts.CmpIDX = sts.Chln + sts.BarCode + d.EXPDT.Value.ToShortDateString(); // expdate used as deliver date
                sts.CPU = d.CPU;
                sts.Qty = d.Qty;
                sts.BoxQty = d.BoxQty;
                sts.POPackQty = d.POPackQty;
                sts.BuyDT = Convert.ToDateTime(d.BuyDT);
                sts.EXPDT = Convert.ToDateTime(d.EXPDT);
                sts.RPU = d.RPU;
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


            }
            BuyOrderPriceApproval b = new();
            b.IsApproved = true;
            b.ApprovedBy = lstbuy.First().UserID;
            b.ApprovedDate = DateTime.Now.Date;
            b.Chln = lstbuy.First().Chln;
            composite.AddRecordSet<BuyOrderPriceApproval>(b, OperationMode.Update, "", "IsApproved,ApprovedBy,ApprovedDate", "Chln", "");
            var res = _dal.InsertUpdateComposite(composite, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return "false";
            }
            return chln;
        }
    }
}
