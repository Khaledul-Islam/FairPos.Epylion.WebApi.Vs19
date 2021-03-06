using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FairPos.Epyllion.Repository.Operations
{
    public interface IReturnRepository
    {
        List<ReasonsForReturn> GetReturnReasonDDL();
        List<TempStockReturnShop> GetTempDataByUser(string userId, string shopId);
        TempStockReturnShop GetTempDataByBarcode(string barcode);
        bool TempReturnSave(Buy model);
        bool TempReturnDelete(TempStockReturnShop model);
        string StockReturnShopSave(List<TempStockReturnShop> model);
        List<TempStockReturnShop> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null);
    }
    public class ReturnRepository : BaseRepository, IReturnRepository
    {
        private readonly IEmailHelperRepository _emailHelperService;
        private readonly ISupplierRepository _supplierService;

        public ReturnRepository(IDBConnectionProvider dBConnectionProvider, IEmailHelperRepository emailHelperService, ISupplierRepository supplierService) : base(dBConnectionProvider)
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
                                  ,[TransferTo]
                                  ,[Reason]
                                  ,[IsDamageStock]
                                  ,[SupID]
                                  ,[IsGiftItemAvailable]
                                  ,[BoxQty]
                                  ,[BoxUOM]
                                  ,[UnitUOM]
                         ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY UserId)  =1)
	                     THEN	(SELECT COUNT(*) FROM TempStockReturnShop) 
	                     ELSE 0 END RecordCount
            FROM TempStockReturnShop";
            _emailHelperService = emailHelperService;
            _supplierService = supplierService;
        }
        public List<ReasonsForReturn> GetReturnReasonDDL()
        {
            string query = "select * from ReasonsForReturn where ReasonsFor='R'";
            var response = _dal.Select<ReasonsForReturn>(query, ref msg).ToList();
            return response;
        }

        public TempStockReturnShop GetTempDataByBarcode(string barcode)
        {
            string query = "select * from TempStockReturnShop where Barcode='" + barcode + "'";
            var response = _dal.Select<TempStockReturnShop>(query, ref msg).FirstOrDefault();
            return response;
        }
        private List<GiftStock> getGiftStock(string sbarcode)
        {
            string query = "select * from GiftStock where Barcode='" + sbarcode + "'";
            return _dal.Select<GiftStock>(query, ref msg).ToList();
        }
        public bool TempReturnSave(Buy model)
        {
            var gf = getGiftStock(model.BarCode);
            TempStockReturnShop otemp = new();
            otemp.Barcode = model.BarCode;
            otemp.RPU = model.RPU;
            otemp.CPU = model.CPU;
            otemp.SBarocde = model.sBarCode;
            otemp.ShopId = model.ShopID;
            otemp.TransferTo = "SUP";
            otemp.SupID = model.SupID;
            otemp.TQty = model.Qty;
            otemp.BoxQty = model.bQty;
            otemp.UserId = model.UserID;
            otemp.ProdcutDescription = model.ProductDescription;
            otemp.Reason = model.reason;
            otemp.BoxUOM = model.UOMName;
            otemp.UnitUOM = model.BoxUOMName;

            otemp.IsGiftItemAvailable = false;
            if (gf != null)
            {
                otemp.IsGiftItemAvailable = true;
            }
            otemp.IsDamageStock = model.IsDamageStock;
            bool response = _dal.Insert<TempStockReturnShop>(otemp, "", "TempId", "", ref msg);
            return response;
        }
        public List<TempStockReturnShop> GetTempDataByUser(string userId, string shopId)
        {
            string query = baseQuery + " where UserId='" + userId + "' and ShopId='" + shopId + "'";
            var response = _dal.Select<TempStockReturnShop>(query, ref msg).ToList();
            return response;
        }
        public List<TempStockReturnShop> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserId='" + UserID + "' AND ShopId='" + ShopID + "' AND ( Barcode like '%" + searchText + @"%' or ProdcutDescription like '%" + searchText + @"%') 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = " where ShopId='" + ShopID + "' AND UserId='" + UserID + "' " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<TempStockReturnShop>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserId='" + UserID + "' AND ShopId='" + ShopID + "' AND (Barcode like '%" + searchText + @"%' or ProdcutDescription like '%" + searchText + @"%') ";
            }
            else
            {
                whereCluase = " where ShopId='" + ShopID + "' AND UserId='" + UserID + "' ";
            }

            query = " select COUNT(*) from TempStockReturnShop " + whereCluase;
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

        public bool TempReturnDelete(TempStockReturnShop model)
        {
            bool response = _dal.Delete<TempStockReturnShop>(model, "UserId,ShopId,Barcode", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }
        public string StockReturnShopSave(List<TempStockReturnShop> model)
        {
            try
            {
                string chln = GetMaxIdWithPrfix2("Chln", "0000000", "0000001", "StockReturnShop", model.First().ShopId);
                List<StockReturnShop> olist = new();
                CompositeModel composite = new();
                foreach (var d in model)
                {
                    StockReturnShop sts = new();
                    sts.BarCode = d.Barcode;
                    sts.sBarcode = d.SBarocde;
                    sts.Chln = chln;
                    sts.CmpIDX = sts.Chln + sts.BarCode;
                    sts.CPU = d.CPU;
                    sts.Qty = d.TQty;
                    sts.BoxQty = d.BoxQty;
                    sts.RcvChln = "";
                    sts.ReturnDt = DateTime.Now;
                    sts.RPU = d.RPU;
                    sts.RcvQty = 0;
                    sts.ShopID = d.ShopId;
                    sts.SupID = d.SupID;
                    sts.TransferTo = d.TransferTo;
                    sts.UserID = d.UserId;
                    sts.IsTransfer = "N";
                    sts.Description = d.Reason;
                    sts.RecevieDt = DateTime.Now;

                    sts.IsDamageStock = d.IsDamageStock;
                    sts.IsGiftItemAvailable = d.IsGiftItemAvailable;
                    olist.Add(sts);

                    if (d.IsDamageStock)
                    {
                        DamageStock damageStock = new();
                        damageStock.balQty = sts.Qty;
                        damageStock.Transfer = "N";
                        damageStock.BarCode = sts.BarCode;
                        damageStock.SrpQty = sts.Qty;
                        composite.AddRecordSet<DamageStock>(damageStock, OperationMode.Update, "", "-balQty,+SrpQty", "BarCode", "");
                    }
                    else
                    {
                        Buy buy = new();
                        buy.balQty = sts.Qty;
                        buy.BarCode = sts.BarCode;
                        buy.sreturn = sts.Qty;
                        buy.Transfer = "N";
                        buy.ShopID = sts.ShopID;

                        composite.AddRecordSet<Buy>(buy, OperationMode.Update, "", "-balQty,+sreturn,Transfer", "BarCode,ShopID", "");

                        GiftStock gf = new();
                        gf.BalQty = sts.Qty;
                        gf.RtnQty = sts.Qty;
                        gf.Barcode = sts.BarCode;
                        composite.AddRecordSet<GiftStock>(gf, OperationMode.Update, "", "-BalQty,+RtnQty", "Barcode", "");
                    }
                }
                composite.AddRecordSet<StockReturnShop>(olist, OperationMode.Insert, "", "", "", "");
                composite.AddRecordSet<TempStockReturnShop>(model.First(), OperationMode.Delete, "Tempid", "", "UserId,ShopId", "");
                if (_dal.InsertUpdateComposite(composite, ref msg))
                {
                    Supplier aSupplier = _supplierService.FindById(model.FirstOrDefault().SupID);
                    List<string> lst = new();
                    lst.Add(aSupplier.RegEmail);
                    _emailHelperService.EmailSender("Returned !!!", lst, new List<string>(), "Returned !!!", null, false);
                    return chln;
                }
                else { return "false"; }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
