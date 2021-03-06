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
using System.Transactions;

namespace FairPos.Epyllion.Repository.Operations
{

    public interface IApprivalCheckRepository
    {
        List<Supplier> GetACSupplierDDL(string UserID, string ShopID);
        List<BuyOrder> GetPendingPONo(string SupID, string ShopID);
        bool RemoveTempArrival(string UserID);
        List<TempArrival> GetTempArrival(string UserID);
        List<TempArrival> GetTempArrivalByChallanNo(string chln);
        bool SaveTempArrival(List<BuyOrder> model);
        bool SaveTempArrivalItem(TempArrival model);
        List<BuyOrder> GetChallanDetails(string POno);
        string SaveArrival(List<TempArrival> model);
        List<TempArrival> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null);
    }

    public class ArrivalCheckRepository : BaseRepository, IApprivalCheckRepository
    {
        public ArrivalCheckRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [TempArrivalId]
                              ,[ARRIVAL_NO]
                              ,[Chln]
                              ,[SupID]
                              ,[sBarCode]
                              ,[BarCode]
                              ,[BarcodeExp]
                              ,[POBoxQty]
                              ,[PoQty]
                              ,[BoxQty]
                              ,[ArrivalQty]
                              ,[DiscPrcnt]
                              ,[VATPrcnt]
                              ,[PrdComm]
                              ,[CPU]
                              ,[RPU]
                              ,[BuyDT]
                              ,[EXPDT]
                              ,[UserID]
                              ,[GIFT_RATIO]
                              ,[GIFT_DESCRIPTION]
                              ,[PrdComPer]
                              ,[PrdComAmnt]
                              ,[AddCost]
                              ,[ItemFullName]
                              ,[BoxUOM]
                              ,[UnitUOM]
                              ,[PrvRcvQty]
                              ,[PrvRcvBox]
                              ,[BoxSize]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY Chln)  =1)
	                     THEN	(SELECT COUNT(*) FROM TempArrival) 
	                     ELSE 0 END RecordCount
                    from TempArrival";
        }
        public List<BuyOrder> GetChallanDetails(string POno)
        {
            string query = @"sp_getBuyOrderDetails '" + POno + "' ";
            var response = _dal.Select<BuyOrder>(query, ref msg).ToList();
            return response;
        }
        public List<Supplier> GetACSupplierDDL(string UserID, string ShopID)
        {
            string query = "select SupID,Supname from Supplier  where SupID in (SELECT DISTINCT SupID FROM BuyOrder   WHERE  IS_ARRIVAL=0 AND IS_CANCEL=0 and ShopID='"+ShopID+"' and UserID='"+UserID+"')";
            var response = _dal.Select<Supplier>(query, ref msg).ToList();
            return response;
        }

        public List<BuyOrder> GetPendingPONo(string SupID, string ShopID)
        {
            string query = @"usp_BuyOrder_GetPOBySupID '" + SupID + "','" + ShopID + "' ";
            var response = _dal.Select<BuyOrder>(query, ref msg).ToList();
            return response;
        }

        public List<TempArrival> GetTempArrival(string UserID)
        {
            string Query = "select * from TempArrival where UserID='" + UserID + "'";
            var response = _dal.Select<TempArrival>(Query, ref msg).ToList();
            return response;
        }
        public List<TempArrival> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null)
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
            var data = _dal.Select<TempArrival>(query, ref msg);

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

            query = " SELECT COUNT(*) FROM [TempArrival] " + whereCluase;
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
        public List<TempArrival> GetTempArrivalByChallanNo(string chln)
        {
            string Query = "select * from TempArrival where Chln='" + chln + "'";
            var response = _dal.Select<TempArrival>(Query, ref msg).ToList();
            return response;
        }

        public bool RemoveTempArrival(string UserID)
        {
            string Query = "DELETE FROM TempArrival WHERE UserID=@UserID";
            _ = _dapper.Execute(Query, new
            {
                @UserID = UserID
            });
            return true;
        }

        public bool SaveTempArrival(List<BuyOrder> model)
        {
            bool response = false;
            List<TempArrival> lstarrival = new List<TempArrival>();
            foreach (var d in model)
            {
                TempArrival arrival = new TempArrival();
                arrival.ArrivalQty = d.Qty - d.RcvArrivalQty;
                arrival.BoxQty = d.BoxQty - d.RcvBoxQty;
                arrival.BarCode = d.BarCode;
                arrival.ItemFullName = d.ItemFullName;
                arrival.BuyDT = DateTime.Now;
                //arrival.Chln = cmbPONo.SelectedValue.ToString();
                arrival.Chln = d.Chln;
                arrival.CPU = d.CPU;
                arrival.DiscPrcnt = d.DiscPrcnt;
                //arrival.EXPDT will be set
                arrival.GIFT_RATIO = 0;
                arrival.GIFT_DESCRIPTION = "";
                arrival.EXPDT = DateTime.Now.AddDays(30);

                arrival.POBoxQty = d.BoxQty;
                arrival.PoQty = d.Qty;
                arrival.PrdComm = d.PrdComm;
                arrival.RPU = d.RPU;
                arrival.sBarCode = d.sBarCode;
                arrival.SupID = d.SupID;
                arrival.UserID = d.UserID;
                arrival.ShopID = d.ShopID;
                arrival.VATPrcnt = d.VATPrcnt;

                StyleSize ss = StyleSizeDetailsByBarcode(arrival.BarCode);
                arrival.BoxUOM = ss.BOXUOMName;
                arrival.UnitUOM = ss.UOMName;
                arrival.PrvRcvQty = d.RcvArrivalQty;
                arrival.PrvRcvBox = d.RcvBoxQty;
                arrival.BoxSize = ss.BoxSize;
                lstarrival.Add(arrival);
            }
            response = _dal.Insert<TempArrival>(lstarrival, "", "TempArrivalId", "", ref msg);
            return response;
        }
        private StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            string query = "SELECT * FROM vStyleSize where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        public bool SaveTempArrivalItem(TempArrival model)
        {
            model.ArrivalQty = model.PackSizeQTY;
            model.BoxQty = model.PackSizeQTY;
            model.BuyDT = DateTime.Now;
            model.Chln = "FREE";
            model.GIFT_RATIO = 0;
            model.GIFT_DESCRIPTION = "";
            model.EXPDT = DateTime.Now.AddDays(30);
            model.PoQty = 0;
            model.POBoxQty = 0;
            model.PrvRcvQty = 0;
            model.PrvRcvBox = 0;
            model.UnitUOM = model.BOXUOMName;
            model.BoxUOM = model.BOXUOMName;
            bool response = _dal.Insert<TempArrival>(model, "", "TempArrivalId", "", ref msg);
            return response;
        }

        public string SaveArrival(List<TempArrival> model)
        {
            try
            {
                string arrival_no = GetMaxIdWithPrfix2("ARRIVAL_NO", "000000", "000001", "Arrival", "A");
                string dn_no = GetMaxIdWithPrfix2("DN_NO", "0000", "0001", "DebitNote", "DN/" + DateTime.Now.Year.ToString() + "/");
                string chln = "";
                bool cbDebitNote = false;
                bool allrcv = true;

                List<Arrival> arrivalList = new List<Arrival>();
                List<DebitNote> debitNotes = new List<DebitNote>();

                foreach (TempArrival d in model)
                {
                    Arrival arrival = new();
                    arrival.ArrivalQty = d.ArrivalQty;
                    arrival.BoxQty = d.BoxQty;
                    arrival.BarCode = d.BarCode;
                    arrival.ItemFullName = d.ItemFullName;
                    arrival.BuyDT = DateTime.Now.Date;
                    arrival.Chln = d.Chln;
                    arrival.CPU = d.CPU;
                    arrival.DiscPrcnt = d.DiscPrcnt;
                    arrival.GIFT_RATIO = d.GIFT_RATIO;
                    arrival.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                    arrival.EXPDT = DateTime.Now.AddDays(30);
                    arrival.PoQty = d.PoQty;
                    arrival.POBoxQty = d.POBoxQty;
                    arrival.PrdComm = d.PrdComm;
                    arrival.RPU = d.RPU;
                    arrival.sBarCode = d.sBarCode;
                    arrival.SupID = d.SupID;
                    arrival.ShopID = d.ShopID;
                    arrival.UserID = d.UserID;
                    arrival.VATPrcnt = d.VATPrcnt;
                    arrival.IS_QC = false;
                    arrival.RefDate = d.rdate;
                    arrival.BarcodeExp = arrival.BarCode.ToString() + arrival.EXPDT.Year.ToString() + arrival.EXPDT.Month.ToString() + arrival.EXPDT.Day.ToString();
                    arrival.ARRIVAL_NO = arrival_no;
                    chln = arrival.Chln;
                    arrival.PrdComPer = 0;
                    arrival.PrdComAmnt = 0;
                    arrival.AddCost = 0;
                    arrival.ReferenceNo = d.RefChallanNo;
                    if (d.ArrivalQty > 0)
                    {
                        StyleSize ss = StyleSizeDetailsByBarcode(arrival.BarCode);
                        double days = (arrival.EXPDT - DateTime.Now.Date).TotalDays;
                        if (ss.ArrivalExpireLimit < 0 || ss.ArrivalExpireLimit == 0)
                        {

                            return "false";
                        }
                        //if (days < ss.ArrivalExpireLimit)
                        //{

                        //    return false;
                        //}

                        arrivalList.Add(arrival);
                        //bool response = _dal.Insert<Arrival>(arrival, "", "ArrivalId", "", ref msg);
                    }

                    if (chln != "Select")
                    {
                        if (d.ArrivalQty + d.PrvRcvQty < d.PoQty)
                        {
                            allrcv = false;
                        }
                    }

                    #region delivery note
                    if (d.cbDebitNote == true && chln != "Select")
                    {
                        if (d.ArrivalQty + d.PrvRcvQty < d.PoQty)
                        {
                            DebitNote dn = new DebitNote();
                            dn.DN_NO = dn_no;
                            dn.BarCode = d.BarCode;
                            dn.BarcodeExp = arrival.BarcodeExp;
                            dn.BuyDT = arrival.BuyDT;
                            dn.Chln = chln;
                            dn.CPU = d.CPU;
                            dn.DiscPrcnt = d.DiscPrcnt;
                            dn.EXPDT = d.EXPDT;
                            dn.GIFT_DESCRIPTION = d.GIFT_DESCRIPTION;
                            dn.GIFT_RATIO = d.GIFT_RATIO;
                            dn.POBoxQty = d.POBoxQty;
                            dn.PoQty = d.PoQty;
                            dn.PrdComm = d.PrdComm;
                            dn.RPU = d.RPU;
                            dn.sBarCode = d.sBarCode;
                            dn.ShortArrivalQty = d.PoQty - (d.ArrivalQty + d.PrvRcvQty);
                            dn.ShortBoxQty = d.POBoxQty - (d.BoxQty + d.PrvRcvBox);
                            dn.SupID = d.SupID;
                            dn.ShopID = d.ShopID;
                            dn.UserID = d.UserID;
                            dn.VATPrcnt = d.VATPrcnt;
                            dn.ARRIVAL_NO = arrival_no;
                            cbDebitNote = d.cbDebitNote;
                            //bool res = _dal.Insert<DebitNote>(dn, "", "DNId", "", ref msg);
                            debitNotes.Add(dn);
                        }
                    }
                    #endregion

                }

                if (!allrcv)
                {
                    if (cbDebitNote)
                        allrcv = true;
                }

                CompositeModel composite = new();
                composite.AddRecordSet<Arrival>(arrivalList, OperationMode.Insert, "ArrivalId", "", "", "");
                if (cbDebitNote == true)
                {
                    composite.AddRecordSet<DebitNote>(debitNotes, OperationMode.Insert, "DNId", "", "", "");
                }
                composite.AddRecordSet<TempArrival>(model.FirstOrDefault(), OperationMode.Delete, "", "", "UserID", "");
                BuyOrder buyOrder = new BuyOrder { Chln = chln, IS_ARRIVAL = allrcv };
                composite.AddRecordSet<BuyOrder>(buyOrder, OperationMode.Update, "", "IS_ARRIVAL", "Chln", "");
                bool resp = _dal.InsertUpdateComposite(composite, ref msg);
                if (resp)
                {
                    return arrival_no;
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
    }
}
