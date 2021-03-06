using Dapper;
using FairPos.Epylion.Models.Requisition;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Requisition
{
    public interface IAutoRequisitionRepository
    {
        List<AutoRequistionTemp> GetTempRequistionData(out string errMsg);
        List<AutoRequistionTemp> GetTempRequistionDataByUser(string user, string shopid, out string errMsg);
        List<AutoRequistion> GetChallanByDateNonReject(string shopid);
        bool GenerateRequistion(string userID,string ShopID, out string errMsg);
        bool RemoveAllTempRequistion(string userId,string shopid, out string errMsg);
        bool UpdateActualQty(AutoRequistionTemp model);
        string AutoRequisitionSave(List<AutoRequistionTemp> olist,out string errMsg);

    }

    public class AutoRequisitionRepository : BaseRepository, IAutoRequisitionRepository
    {
        public AutoRequisitionRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [CmpIDX]
                                  ,[Chln]
                                  ,[SupID]
                                  ,[sBarCode]
                                  ,[BarCode]
                                  ,[BoxQty]
                                  ,[Qty]
                                  ,[sQty]
                                  ,[DiscPrcnt]
                                  ,[VATPrcnt]
                                  ,[PrdComm]
                                  ,[CPU]
                                  ,[RPU]
                                  ,[BuyDT]
                                  ,[EXPDT]
                                  ,[UserID]
                                  ,[PrdDescription]
                                  ,[BoxUOM]
                                  ,[UnitUOM]
                                  ,[PrdID]
                                  ,[POPackQty]
                                  ,[PackUOM]
                                  ,[BalQty]
                                  ,[BoxSize]
                                  ,[MinOrder]
                                  ,[POPackSize]
                                  ,[ActualBoxQty]
                                  ,[Remarks]
                                  ,[ShopID]
                              FROM [AutoRequistionTemp]";
        }

        public string AutoRequisitionSave(List<AutoRequistionTemp> olist, out string errMsg)
        {
            errMsg = string.Empty;
            if (olist.Count == 0)
            {
                errMsg="No data for save";
                return "false";
            }
            try
            {
                string chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "AutoRequistion", "RQ/" + olist.First().ShopID + "/" + DateTime.Now.Year.ToString() + "/");
                List<AutoRequistion> lstAutoReq = new();
                foreach (var d in olist)
                {
                    AutoRequistion req = new();
                    req.ActualBoxQty = d.ActualBoxQty;
                    req.ApprovedBoxQty = 0;
                    req.BalQty = d.BalQty;
                    req.BarCode = d.BarCode;
                    req.ApproveStatus = "NEW";
                    req.BoxQty = d.BoxQty;
                    req.BoxSize = d.BoxSize;
                    req.BoxUOM = d.BoxUOM;
                    req.BuyDT = DateTime.Now;
                    req.Chln = chln;
                    req.CmpIDX = chln + d.BarCode;
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
                    req.IsPODone = false;
                    req.Remarks = d.Remarks;
                    req.ShopID = d.ShopID;
                    lstAutoReq.Add(req);
                }
                var response = _dal.Insert<AutoRequistion>(lstAutoReq, "", "", "", ref msg);
                if (response == false || !string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                RemoveAllTempRequistion(olist.First().UserID, olist.First().ShopID, out errMsg);
                if (response)
                {
                    return chln;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GenerateRequistion(string userID,string ShopID, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                List<AutoRequistionTemp> olist = GetTempRequistionData(out string err);
                if (olist != null)
                {
                    errMsg = "Another user alreay processing this record";
                    return false;
                }
                List<AutoRequistion> olistReq = GetChallanByDateNonReject(ShopID);

                if (olistReq != null)
                {
                    errMsg = "A requistion already made in this month";
                    return false;
                }
                string fromDate = DateTime.Now.AddDays(-30).ToShortDateString();
                string toDate = DateTime.Now.ToShortDateString();                
                string query = "SP_GenerateAutoRequistion '" + fromDate + "','" + toDate + "','" + userID + "','"+ ShopID + "' ";
                var response = _dapper.Execute(query);

            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            return true;
        }
        //private List<AutoRequistionTemp> trailItemRequisition(string UserID)
        //{
        //    string query = "select * from StyleSize  where isTrail=1";
        //    var data = _dal.Select<StyleSize>(query, ref msg);
        //    List<AutoRequistionTemp> lstReq = new();
        //    if (data.Count > 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            AutoRequistionTemp temp = new();
        //            temp.CmpIDX = item.sBarcode + item.Barcode;
        //            temp.Chln = "c";
        //            temp.SupID = item.SupID;
        //            temp.UserID = UserID;
        //            temp.BarCode = item.Barcode;
        //            temp.sBarCode = item.sBarcode;
        //            temp.BoxQty = 1/item.BoxSize;
        //            temp.Qty = Convert.ToDecimal(item.balQty);
        //            temp.DiscPrcnt = item.DiscPrcnt;
        //            temp.VATPrcnt = item.VATPrcnt;
        //            temp.PrdComm = item.PrdComm;
        //            temp.CPU = item.CPU;
        //            temp.RPU = item.RPU;
        //            //temp.BuyDT = item.RPU;
        //            //temp.EXPDT = item.dt;
        //            temp.BoxUOM = item.BOXUOMName;
        //            //temp.UnitUOM = item.UOMName;
        //            temp.PrdID = item.PrdID;
        //           // temp.POPackQty = item.POPackSize;
        //            //temp.PackUOM = item.PackUiomName;
        //            temp.BalQty = Convert.ToDecimal(item.balQty);
        //            //temp.sQty = Convert.ToDecimal(item.balQty);
        //            temp.BoxSize = item.BoxSize;
        //            temp.MinOrder = item.MinOrder;
        //            temp.POPackSize = item.POPackSize;
        //            //temp.ActualBoxQty = item.b;
        //            temp.PrdDescription = item.PrdName+ item.ItemNameBangla;
        //            lstReq.Add(temp);
        //        }
        //        var response = _dal.Insert<AutoRequistionTemp>(lstReq, "", "", "", ref msg);
        //        if(response==false|| !string.IsNullOrEmpty(msg))
        //        {
        //            return null;
        //        }
                
        //    }
        //    return lstReq;
        //}
        public List<AutoRequistion> GetChallanByDateNonReject(string shopid)
        {
            string fromDate = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
            int lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            string toDay = DateTime.Now.Month.ToString() + "/" + lastDay.ToString() + "/" + DateTime.Now.Year.ToString();
            string query = @"select distinct(Chln) from AutoRequistion where ShopID='"+shopid+"' AND BuyDT between '" + fromDate + "' and '" + toDay + "' and ApproveStatus !='REJECT'";
            var data = _dal.Select<AutoRequistion>(query, ref msg);
            if (data == null || data.Count <= 0)
            {
                return null;
            }
            return data;
        }

        public List<AutoRequistionTemp> GetTempRequistionData(out string errMsg)
        {
            errMsg = string.Empty;
            var response = _dal.Select<AutoRequistionTemp>(baseQuery, ref msg);
            if (response == null || response.Count == 0)
            {
                errMsg = "No record found.";
                return null;
            }
            return response;

        }

        public List<AutoRequistionTemp> GetTempRequistionDataByUser(string user, string shopid, out string errMsg)
        {
            errMsg = string.Empty;
            var response = _dal.Select<AutoRequistionTemp>(baseQuery + " where UserID='" + user + "' AND ShopID='"+shopid+"'", ref msg);
            if (response == null || response.Count == 0)
            {
                errMsg = "No record found.";
                return null;
            }
            return response;
        }

        public bool RemoveAllTempRequistion(string userId, string shopid, out string errMsg)
        {
            errMsg = string.Empty;
            string query = "delete  from AutoRequistionTemp where UserID='" + userId + "' AND ShopID='"+ shopid + "'; SELECT @@ROWCOUNT ";
            int objResult = _dapper.Execute(query);
            if (objResult <= 0)
            {
                errMsg = "No row affected";
                return false;
            }
            return true;

        }

        public bool UpdateActualQty(AutoRequistionTemp model)
        {
            if (model == null)
            {
                return false;
            }
            var response = _dal.Update<AutoRequistionTemp>(model, "ActualBoxQty", "BarCode", "", ref msg);
            if(response==false|| !string.IsNullOrEmpty(msg))
            {
                return false;
            }
            return response;
        }
    }
}
