﻿using Dapper;
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
    public interface IArrivalEditRepository
    {
        List<string> GetArrivalNoQC(string UserID, string ShopID);
        List<TempArrivalUpdate> GetTempArrivalUpdateByUser(string UserId);
        List<TempArrivalUpdate> GetsTempArrivalUpdate();
        List<TempArrivalUpdate> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null);
        bool LoadTempArrivalUpdate(string arrivalNo, string UserID, out string errMsg);
        bool updateTmpArrival(TempArrivalUpdate item, out string errMsg);
        string updateArrival(TempArrivalUpdate item, out string errMsg);
        bool deleteTmpArrival(TempArrivalUpdate item);
    }

    public class ArrivalEditRepository : BaseRepository, IArrivalEditRepository
    {
        private readonly IApprivalCheckRepository _arrivalsService;
        public ArrivalEditRepository(IDBConnectionProvider dBConnectionProvider, IApprivalCheckRepository arrivalsService) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT *,CASE WHEN (ROW_NUMBER() OVER (ORDER BY Chln)  =1)
	                     THEN	(SELECT COUNT(*) FROM TempArrivalUpdate) 
	                     ELSE 0 END RecordCount
                    from TempArrivalUpdate";
            _arrivalsService = arrivalsService;
        }
        public List<string> GetArrivalNoQC(string UserID, string ShopID)
        {
            string query = @"SELECT DISTINCT ARRIVAL_NO FROM dbo.Arrival WHERE IS_QC=0
                        and ShopID='" + ShopID + "' and UserID='" + UserID + "'  AND ARRIVAL_NO NOT IN ( SELECT DISTINCT ARRIVAL_NO FROM dbo.Arrival WHERE IS_QC=1)";

            var response = _dapper.Query<string>(query).ToList();
            if (response.Count == 0)
            {
                return null;
            }

            return response;
        }

        public List<TempArrivalUpdate> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserID='" + UserID + "' AND ( BarCode like '%" + searchText + @"%' or ItemFullName like '%" + searchText + @"%') 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = " where UserID='" + UserID + "' " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<TempArrivalUpdate>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserID='" + UserID + "' AND (BarCode like '%" + searchText + @"%' or ItemFullName like '%" + searchText + @"%') ";
            }
            else
            {
                whereCluase = " where UserID='" + UserID + "' ";
            }

            query = " SELECT COUNT(*) FROM [TempArrivalUpdate] " + whereCluase;
            var count = _dal.SelectFirstColumn(query, ref msg);
            if (data != null && data.Count > 0)
            {
                int _count;
                int.TryParse(count, out _count);
                data[0].RecordFilter = _count;
            }
            #endregion

            msgs = msg;
            return data;
        }

        public List<TempArrivalUpdate> GetsTempArrivalUpdate()
        {
            string query = "select * from TempArrivalUpdate";
            var response = _dal.Select<TempArrivalUpdate>(query, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }
        private List<Arrival> GetChallanDetails(string arrivalNo)
        {
            string query = "select * from Arrival where ARRIVAL_NO='" + arrivalNo + "'";
            var response = _dal.Select<Arrival>(query, ref msg).ToList();
            if (response == null)
            {
                return null;
            }
            return response;
        }
        public List<TempArrivalUpdate> GetTempArrivalUpdateByUser(string UserId)
        {
            string query = "select * from TempArrivalUpdate where UserID='" + UserId + "'";
            var response = _dal.Select<TempArrivalUpdate>(query, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return response;
        }
        private StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var query = "select * FROM vStyleSize where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        public bool LoadTempArrivalUpdate(string arrivalNo, string UserID, out string errMsg)
        {
            errMsg = string.Empty;
            List<Arrival> orders = GetChallanDetails(arrivalNo);
            if (orders.Count == 0)
            {
                errMsg = "No challan found";
                return false;
            }
            List<TempArrivalUpdate> arrivals = GetsTempArrivalUpdate().Where(a=>a.ARRIVAL_NO==arrivalNo && a.UserID!= UserID).ToList();
            if (arrivals.Count > 0)
            {

                errMsg = "A PO is already processing by another user";
                return false;
            }
            CompositeModel composite = new();
            foreach (Arrival d in orders)
            {
                StyleSize ss = StyleSizeDetailsBySbarcode(d.sBarCode);
                if (ss == null)
                {
                    errMsg = "Item not found";
                    return false;
                }
                TempArrivalUpdate arrival = new();
                arrival.ArrivalQty = d.ArrivalQty;
                arrival.BoxQty = d.BoxQty;
                arrival.BarCode = d.BarCode;
                arrival.ItemFullName = ss.ItemFullName;
                arrival.BuyDT = d.BuyDT;
                arrival.Chln = d.Chln;
                arrival.CPU = d.CPU;
                arrival.DiscPrcnt = d.DiscPrcnt;
                arrival.ARRIVAL_NO = d.ARRIVAL_NO;
                arrival.GIFT_RATIO = d.GIFT_RATIO;
                arrival.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                arrival.EXPDT = d.EXPDT;
                arrival.POBoxQty = d.POBoxQty;
                arrival.PoQty = d.PoQty;
                arrival.PrdComm = d.PrdComm;
                arrival.RPU = d.RPU;
                arrival.sBarCode = d.sBarCode;
                arrival.SupID = d.SupID;
                arrival.UserID = UserID;
                arrival.VATPrcnt = d.VATPrcnt;
                arrival.BoxUOM = ss.BOXUOMName;
                arrival.UnitUOM = ss.UOMName;
                arrival.PrvRcvQty = d.ArrivalQty;
                arrival.PrvRcvBox = d.BoxQty;
                arrival.BoxSize = ss.BoxSize;
                arrival.RefDate = d.RefDate;
                arrival.RefNo = d.ReferenceNo;
                composite.AddRecordSet<TempArrivalUpdate>(arrival, OperationMode.Insert, "TempArrivalId", "", "", "");
            }
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool updateTmpArrival(TempArrivalUpdate item, out string errMsg)
        {
            errMsg = string.Empty;
            var response = _dal.Update<TempArrivalUpdate>(item, "EXPDT,CPU,RPU,ArrivalQty,BoxQty,GIFT_RATIO,GIFT_DESCRIPTION", "BarCode", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool deleteTmpArrival(TempArrivalUpdate item)
        {
            var response = _dal.Delete<TempArrivalUpdate>(item, "UserID", "", ref msg);
            if (response == false)
            {
                return false;
            }
            return response;
        }
        private StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            string query = "SELECT * FROM vStyleSize where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }
        public string updateArrival(TempArrivalUpdate item, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                CompositeModel composite = new();
                List<TempArrivalUpdate> arrivals = GetTempArrivalUpdateByUser(item.UserID);
                if (arrivals.Count == 0)
                {
                    errMsg = "সংরক্ষণ কোনো ডেটা নেই";
                    return "false";
                }
                List<BuyOrder> orders = _arrivalsService.GetChallanDetails(arrivals[0].Chln);
                List<Arrival> CurrentArriavalRecord = GetChallanDetails(item.ARRIVAL_NO);
                foreach (TempArrivalUpdate d in arrivals)
                {
                    if (d.BoxQty > d.POBoxQty)
                    {
                        errMsg = "Receive qty can not greater then PO qty , Barcode: " + d.BarCode;
                        return "false";
                    }
                    BuyOrder order = orders.Find(m => m.BarCode == d.BarCode);
                    Arrival arivalCurrent = CurrentArriavalRecord.Find(m => m.BarCode == d.BarCode);
                    if (order != null)
                    {
                        if (d.BoxQty + (order.RcvArrivalQty - arivalCurrent.ArrivalQty) > d.POBoxQty)
                        {
                            errMsg = "Receive qty can not greater then PO qty , Barcode: " + d.BarCode + "\r\n already ariaval qty in another arrival no" + (order.RcvArrivalQty - arivalCurrent.ArrivalQty).ToString();
                            return "false";
                        }
                    }
                }
                string chln = "";
                bool allrcv = true;
                foreach (TempArrivalUpdate d in arrivals)
                {
                    Arrival arrival = new();
                    arrival.ArrivalQty = d.ArrivalQty;
                    arrival.BoxQty = d.BoxQty;
                    arrival.BarCode = d.BarCode;
                    arrival.ItemFullName = d.ItemFullName;
                    arrival.BuyDT = item.BuyDT;
                    arrival.Chln = d.Chln;
                    arrival.CPU = d.CPU;
                    arrival.DiscPrcnt = d.DiscPrcnt;
                    arrival.GIFT_RATIO = d.GIFT_RATIO;
                    arrival.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                    arrival.EXPDT = d.EXPDT; //DateTime.Now.AddDays(30);
                    arrival.PoQty = d.PoQty;
                    arrival.POBoxQty = d.POBoxQty;
                    arrival.PrdComm = d.PrdComm;
                    arrival.RPU = d.RPU;
                    arrival.sBarCode = d.sBarCode;
                    arrival.SupID = d.SupID;
                    arrival.UserID = item.UserID;
                    arrival.VATPrcnt = d.VATPrcnt;
                    arrival.IS_QC = false;
                    arrival.RefDate = item.RefDate;
                    arrival.BarcodeExp = arrival.BarCode + arrival.EXPDT.Year.ToString() + arrival.EXPDT.Month.ToString("00") + arrival.EXPDT.Day.ToString("00");
                    arrival.ARRIVAL_NO = d.ARRIVAL_NO;
                    chln = arrival.Chln;
                    arrival.PrdComPer = 0;
                    arrival.PrdComAmnt = 0;
                    arrival.AddCost = 0;
                    arrival.ReferenceNo = item.RefNo;
                    StyleSize ss = StyleSizeDetailsByBarcode(arrival.BarCode);
                    double days = (arrival.EXPDT - DateTime.Now.Date).TotalDays;

                    if (ss.ArrivalExpireLimit == null || ss.ArrivalExpireLimit.HasValue == false)
                    {
                        errMsg = "Arrival expiry not set for this product for barocde :" + arrival.BarCode;
                        return "false";
                    }
                    //if (days < ss.ArrivalExpireLimit)
                    // {
                    //    errMsg = "Expiry date limit is not valid for barocde :" + arrival.BarCode;
                    //   return false;
                    // }
                    string cols = "ArrivalQty,BoxQty,GIFT_DESCRIPTION,GIFT_RATIO,RPU,CPU,ReferenceNo,RefDate,BuyDT,EXPDT,BarcodeExp";
                    composite.AddRecordSet<Arrival>(arrival, OperationMode.Update, "ArrivalId", cols, "ARRIVAL_NO,BarCode", "");

                    #region delivery note
                    decimal qty = (d.PrvRcvQty - d.ArrivalQty);
                    decimal Boxqty = (d.PrvRcvBox - d.BoxQty);
                    DebitNote dataPrv = new();
                    dataPrv.ShortArrivalQty += qty;
                    dataPrv.ShortBoxQty += Boxqty;
                    dataPrv.ARRIVAL_NO = arrival.ARRIVAL_NO;
                    dataPrv.BarCode = arrival.BarCode;
                    composite.AddRecordSet<DebitNote>(dataPrv, OperationMode.Update, "DNId", "+ShortArrivalQty,+ShortBoxQty", "ARRIVAL_NO,BarCode", "");
                    #endregion

                }
                composite.AddRecordSet<TempArrivalUpdate>(item, OperationMode.Delete, "TempArrivalId", "", "UserID", "");
                BuyOrder bo = new();
                bo.IS_ARRIVAL = allrcv;
                bo.Chln = chln;
                composite.AddRecordSet<BuyOrder>(bo, OperationMode.Update, "", "IS_ARRIVAL", "Chln", "");

                var response = _dal.InsertUpdateComposite(composite, ref msg);
                if (response == false || !string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                return item.ARRIVAL_NO;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
