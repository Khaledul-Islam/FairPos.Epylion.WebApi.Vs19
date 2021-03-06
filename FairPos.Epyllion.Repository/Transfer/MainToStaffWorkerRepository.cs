using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Transfer
{
    public interface IMainToStaffWorkerRepository
    {
        List<Buy> GetTransferProducts(string SupId, string ShopID);
        List<Buy> GetTransferNewBarcode(string sBarcode, decimal qty);
        Buy GetTransferBarcodeExp(string expbarcode);
        TempStockTransfer GetTempStock(string Barcode, string stockType);
        List<TempStockTransfer> GetTempStocks();
        List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null);
        bool SaveTempStockTransfer(Buy model);
        bool DeleteTempStockTransfer(TempStockTransfer model);
        string SaveStockTransfer(List<TempStockTransfer> model);

    }

    public class MainToStaffWorkerRepository : BaseRepository, IMainToStaffWorkerRepository
    {
        public MainToStaffWorkerRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT   [TempId]
                                  ,[UserId]
                                  ,[ShopId]
                                  ,[PrvCusID]
                                  ,[CustomerName]
                                  ,[CustomerMobile]
                                  ,[SBarocde]
                                  ,[Barcode]
                                  ,[TQty]
                                  ,[CPU]
                                  ,[RPU]
                                  ,[ProdcutDescription]
                                  ,[TransferFrom]
                                  ,[TransferTo]
                                  ,[Reason]
                                  ,[IsDamageStock]
                                  ,[SupID]
                                  ,[IsGiftItemAvailable]
                                  ,[BoxQty]
                                  ,[BoxUOM]
                                  ,[UnitUOM]
                                  ,[EXPDT]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY Barcode)  =1)
	                     THEN	(SELECT COUNT(*) FROM TempStockTransfer) 
	                     ELSE 0 END RecordCount
                    FROM TempStockTransfer";
        }
        //1
        public List<Buy> GetTransferProducts(string SupId, string ShopID)
        {
            string query = @"SP_GetStockByOrgBarcode 'All','All','" + ShopID + "' ";
            var response = _dal.Select<SupportBuy>(query, ref msg).ToList();
            if (response.Count <= 0)
            {
                return null;
            }
            List<Buy> buyList = new();
            foreach (var item in response)
            {
                Buy buy = new();
                buy.balQty = item.BalQty;
                buy.BarCode = item.Barcode;
                buy.sBarCode = item.sBarcode;
                buy.PrdID = item.PrdID;
                buy.PrdName = item.PrdName;
                buy.SupID = item.SupID;
                buy.SupName = item.Supname;
                buy.UOMName = item.UOMName;
                buy.ProductDescription = item.ItemName + item.UOMName;
                buy.RPU = item.RPU;
                buy.AutoSale = item.AutoSale;
                buy.ProductDescriptionBangla = item.ItemNameBangla;
                buy.BoxSize = item.BoxSize;
                buy.IsConversionItem = item.IsConverationItem;
                buy.SaleBalQty = buy.balQty / buy.BoxSize;
                buyList.Add(buy);
            }
            if (string.IsNullOrEmpty(SupId))
            {
                return buyList.Where(m => m.IsConversionItem == false).ToList();
            }
            List<Buy> data = buyList.Where(m => m.IsConversionItem == false && m.SupID == SupId).ToList();
            return data;

        }

        private StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var query = "select * from vStyleSize where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        //2
        public List<Buy> GetTransferNewBarcode(string sBarcode, decimal qty)
        {
            StyleSize ss = StyleSizeDetailsBySbarcode(sBarcode);
            if (ss == null)
            {
                return new List<Buy>();
            }

            if (qty >= 999999999999)
            {
                qty = 999999999999;
            }
            else
            {
                qty *= ss.BoxSize;
            }
            string query = @"SP_GetNewBarcodeBySbarocde '" + sBarcode + "' ," + qty + "";
            var response = _dal.Select<SupportBuy>(query, ref msg).ToList();
            if (response.Count <= 0)
            {
                return null;
            }
            List<Buy> buyList = new();
            foreach (var item in response)
            {
                Buy buy = new();
                buy.balQty = item.BalQty;
                buy.BarCode = item.Barcode;
                buy.sBarCode = item.sBarcode;
                buy.PrdID = item.PrdID;
                buy.PrdName = item.PrdName;
                buy.SupID = item.SupID;
                buy.SupName = item.Supname;
                buy.UOMName = item.UOMName;
                buy.BoxUOMName = item.BoxUOMName;
                buy.ProductDescription = item.ItemFullName;
                buy.ProductDescriptionBangla = item.ItemFullNameBangla;
                buy.RPU = item.RPU;
                buy.CPU = item.CPU;
                buy.VATPrcnt = item.VATPrcnt;
                buy.EXPDT = item.EXPDT;
                buy.sQty = item.AppliedQty;
                buy.BoxSize = item.BoxSize;
                buy.SaleBalQty = buy.balQty / buy.BoxSize;
                buy.ItemWeight = item.ItemWeight;
                buyList.Add(buy);
            }
            return buyList;
        }

        //3
        public Buy GetTransferBarcodeExp(string expbarcode)
        {
            string query = @"SP_GetByBarcodeExp '" + expbarcode + "' ";
            var item = _dal.Select<SupportBuy>(query, ref msg).FirstOrDefault();
            if (item == null)
            {
                return new Buy();
            }
            Buy buy = new Buy();
            buy.balQty = item.BalQty;
            buy.BarCode = item.Barcode;
            buy.sBarCode = item.sBarcode;
            buy.PrdID = item.PrdID;
            buy.PrdName = item.PrdName;
            buy.SupID = item.SupID;
            buy.SupName = item.Supname;
            buy.UOMName = item.UOMName;
            buy.ProductDescription = item.ItemFullName;
            buy.ProductDescriptionBangla = item.ItemFullNameBangla;
            buy.RPU = item.RPU;
            buy.CPU = item.CPU;
            buy.VATPrcnt = item.VATPrcnt;
            buy.EXPDT = item.EXPDT;
            buy.BoxUOMName = item.BoxUOMName;
            buy.BoxSize = item.BoxSize;
            buy.IsConversionItem = item.IsConverationItem;
            buy.SaleBalQty = buy.balQty / buy.BoxSize;

            return buy;
        }

        public TempStockTransfer GetTempStock(string Barcode, string stockType)
        {
            string query = baseQuery + " where Barcode='" + Barcode + "' and TransferTo='" + stockType + "'";
            var response = _dal.Select<TempStockTransfer>(query, ref msg).FirstOrDefault();
            return response;
        }

        public List<TempStockTransfer> GetTempStocks()
        {
            var response = _dal.Select<TempStockTransfer>(baseQuery, ref msg).ToList();
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);
            return response;
        }
        private List<GiftStock> getGiftStock(string sbarcode)
        {
            string query = "select * from GiftStock where Barcode='" + sbarcode + "'";
            return _dal.Select<GiftStock>(query, ref msg).ToList();
        }
        public bool SaveTempStockTransfer(Buy model)
        {
            var gf = getGiftStock(model.BarCode).FirstOrDefault();

            TempStockTransfer otemp = new();

            otemp.Barcode = model.BarCode;
            otemp.RPU = model.RPU;
            otemp.CPU = model.CPU;
            otemp.EXPDT = model.EXPDT;
            otemp.SBarocde = model.sBarCode;
            otemp.ShopId = model.ShopID;
            otemp.TransferFrom = "Main Stock";
            otemp.TransferTo = model.stocktype;
            otemp.SupID = model.SupID;
            otemp.TQty = model.reqqty;
            otemp.BoxQty = model.reqbox;
            otemp.UserId = model.UserID;
            otemp.ProdcutDescription = model.ProductDescription;
            otemp.BoxUOM = model.UOMName;
            otemp.UnitUOM = model.BoxUOMName;
            otemp.IsGiftItemAvailable = false;
            if (gf != null)
            {
                otemp.IsGiftItemAvailable = true;
            }
            CompositeModel composite = new();
            string cols = "Barcode,RPU,CPU,EXPDT,SBarocde,ShopId,TransferFrom,TransferTo,SupID,+TQty,BoxQty,UserId,ProdcutDescription,BoxUOM,UnitUOM,IsGiftItemAvailable";
            composite.AddRecordSet<TempStockTransfer>(otemp, OperationMode.InsertOrUpdaet, "TempId", cols, "Barcode,ShopId,UserId,TransferFrom", "");

            var response = _dal.InsertUpdateComposite(composite, ref msg);
            return response;
        }
        public List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where TransferFrom='" + stocktype + "' AND UserId='" + UserID + "' AND ShopId='" + ShopID + "' AND ( Barcode like '%" + searchText + @"%' or ProdcutDescription like '%" + searchText + @"%') 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = " where TransferFrom='" + stocktype + "' AND ShopId='" + ShopID + "' AND UserId='" + UserID + "' " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<TempStockTransfer>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where TransferFrom='" + stocktype + "' AND UserId='" + UserID + "' AND ShopId='" + ShopID + "' AND (Barcode like '%" + searchText + @"%' or ProdcutDescription like '%" + searchText + @"%') ";
            }
            else
            {
                whereCluase = " where TransferFrom='" + stocktype + "' AND ShopId='" + ShopID + "' AND UserId='" + UserID + "' ";
            }

            query = " select COUNT(*) from TempStockTransfer " + whereCluase;
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

        public bool DeleteTempStockTransfer(TempStockTransfer model)
        {
            var response = _dal.Delete<TempStockTransfer>(model, "UserId,ShopId,SBarocde,Barcode,TransferFrom", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                throw new Exception(msg);
            }
            return response;
        }

        public string SaveStockTransfer(List<TempStockTransfer> model)
        {
            try
            {
                string chln = "";
                string prefix = "MT" + DateTime.Now.ToShortDateString();
                chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "StockTransfer", prefix);
                CompositeModel composite = new();
                foreach (var d in model)
                {
                    StockTransfer sts = new();
                    sts.BarCode = d.Barcode;
                    sts.sBarcode = d.SBarocde;
                    sts.Chln = chln;
                    sts.CmpIDX = sts.Chln + sts.BarCode;
                    sts.CPU = d.CPU;
                    sts.TransferQty = d.TQty;
                    sts.BoxQty = d.BoxQty;
                    sts.TransferDt = DateTime.Now;
                    sts.RPU = d.RPU;
                    sts.ShopID = d.ShopId;
                    sts.SupID = d.SupID;
                    sts.UserID = d.UserId;
                    sts.Description = d.Reason;
                    sts.TransferFrom = d.TransferFrom;
                    sts.TransferTo = d.TransferTo;
                    sts.EXPDT = d.EXPDT;
                    sts.IsDamageStock = d.IsDamageStock;
                    sts.IsGiftItemAvailable = d.IsGiftItemAvailable;

                    composite.AddRecordSet<StockTransfer>(sts, OperationMode.Insert, "", "", "", "");

                    //transfer out
                    Buy b = new();
                    b.BarCode = d.Barcode;
                    b.TransferOut = d.TQty;
                    b.balQty = d.TQty;
                    composite.AddRecordSet<Buy>(b, OperationMode.InsertOrUpdaet, "", "+TransferOut,-balQty", "BarCode", "");
                    // transfer in
                    if (d.TransferTo == "Staff Stock")
                    {
                        BuyStaff buystaff = new();
                        buystaff.CMPIDX = d.SBarocde + d.Barcode; //****
                        buystaff.balQty = d.TQty; //****
                        buystaff.BarCode = d.Barcode;
                        buystaff.sBarCode = d.SBarocde;
                        buystaff.Chln = chln;
                        buystaff.BuyDT = DateTime.Now;
                        buystaff.CPU = d.CPU;
                        buystaff.LCPU = d.CPU;
                        buystaff.RPU = d.RPU;
                        buystaff.RPP = 0;
                        buystaff.WSP = 0;
                        buystaff.WSQ = 0;
                        buystaff.DiscPrcnt = 0;
                        buystaff.VATPrcnt = 0;
                        buystaff.PrdComm = 0;
                        buystaff.Qty = 0;
                        buystaff.CQty = 0;
                        buystaff.bQty = 0;
                        buystaff.TrfQty = 0;
                        buystaff.sreturn = 0;
                        buystaff.sreturn = 0;
                        buystaff.sQty = 0;
                        buystaff.rQty = 0;
                        buystaff.SrpQty = 0;
                        buystaff.dmlqty = 0;
                        buystaff.InvQty = 0;
                        buystaff.SupID = d.SupID;
                        buystaff.EXPDT = d.EXPDT;
                        buystaff.Transfer = "N";
                        buystaff.SupRef = "N/A";
                        buystaff.UserID = d.UserId;
                        buystaff.ShopID = d.ShopId;
                        buystaff.Point = 0;
                        buystaff.Reorder = 0;
                        buystaff.ZoneID = "";
                        buystaff.BarocdeDate = d.EXPDT;
                        buystaff.WriteOff = 0;
                        buystaff.WriteOn = 0;
                        buystaff.TransferIn = d.TQty; //*****
                        buystaff.TransferOut = 0;

                        string cols = "CMPIDX,+balQty,BarCode,sBarCode,Chln,BuyDT,CPU,LCPU,RPU,RPP,WSP,WSQ,DiscPrcnt,VATPrcnt,PrdComm,Qty,CQty,bQty,TrfQty,sreturn," +
                            "sreturn,sQty,rQty,SrpQty,dmlqty,InvQty,SupID,EXPDT,Transfer,SupRef,UserID,Point,Reorder,ZoneID,BarocdeDate,WriteOff,WriteOn,+TransferIn,TransferOut,ShopID";

                        composite.AddRecordSet<BuyStaff>(buystaff, OperationMode.InsertOrUpdaet, "", cols, "BarCode,ShopID", "");

                    }
                    else if (d.TransferTo == "Worker Stock")
                    {
                        BuyWorker buyworker = new();
                        buyworker.CMPIDX = d.SBarocde + d.Barcode; //****
                        buyworker.balQty = d.TQty; //****
                        buyworker.BarCode = d.Barcode;
                        buyworker.sBarCode = d.SBarocde;
                        buyworker.Chln = chln;
                        buyworker.BuyDT = DateTime.Now;
                        buyworker.CPU = d.CPU;
                        buyworker.LCPU = d.CPU;
                        buyworker.RPU = d.RPU;
                        buyworker.RPP = 0;
                        buyworker.WSP = 0;
                        buyworker.WSQ = 0;
                        buyworker.DiscPrcnt = 0;
                        buyworker.VATPrcnt = 0;
                        buyworker.PrdComm = 0;
                        buyworker.Qty = 0;
                        buyworker.CQty = 0;
                        buyworker.bQty = 0;
                        buyworker.TrfQty = 0;
                        buyworker.sreturn = 0;
                        buyworker.sreturn = 0;
                        buyworker.sQty = 0;
                        buyworker.rQty = 0;
                        buyworker.SrpQty = 0;
                        buyworker.dmlqty = 0;
                        buyworker.InvQty = 0;
                        buyworker.SupID = d.SupID;
                        buyworker.ShopID = d.ShopId;
                        buyworker.EXPDT = d.EXPDT;
                        buyworker.Transfer = "N";
                        buyworker.SupRef = "N/A";
                        buyworker.UserID = d.UserId;
                        buyworker.Point = 0;
                        buyworker.Reorder = 0;
                        buyworker.ZoneID = "";
                        buyworker.BarocdeDate = d.EXPDT;
                        buyworker.WriteOff = 0;
                        buyworker.WriteOn = 0;
                        buyworker.TransferIn = d.TQty; //*****
                        buyworker.TransferOut = 0;
                        string cols = "CMPIDX,+balQty,BarCode,sBarCode,Chln,BuyDT,CPU,LCPU,RPU,RPP,WSP,WSQ,DiscPrcnt,VATPrcnt,PrdComm,Qty,CQty,bQty,TrfQty,sreturn,sreturn,sQty,rQty," +
                            "SrpQty,dmlqty,InvQty,SupID,EXPDT,Transfer,SupRef,UserID,Point,Reorder,ZoneID,BarocdeDate,WriteOff,WriteOn,+TransferIn,TransferOut,ShopID";
                        composite.AddRecordSet<BuyWorker>(buyworker, OperationMode.InsertOrUpdaet, "", cols, "BarCode,ShopID", "");

                    }


                }
                composite.AddRecordSet<TempStockTransfer>(model.First(), OperationMode.Delete, "TempId", "", "UserId,ShopId", "");
                var response = _dal.InsertUpdateComposite(composite, ref msg);
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
    }
}
