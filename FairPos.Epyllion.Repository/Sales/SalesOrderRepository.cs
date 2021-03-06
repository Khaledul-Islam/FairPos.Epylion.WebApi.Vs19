using Dapper;
using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Sales
{
    public interface ISalesOrderRepository
    {
        List<SALES_ORDER> GetPendingPrintItems(string stockType);
        List<SALES_ORDER> GetOrderDetails(string so_no);
        BuyWorker GetProductInfoByBarcodeBuyWorker(string barcode, string supplierId);
        BuyStaff GetProductInfoByBarcodeBuyStaff(string barcode, string supplierId);
        Buy GetProductInfoByBarcode(string barcode, string supplierId);
        List<SALES_ORDER> GetPendingItemsByEmp(string empId, bool isAccApproved);
    }

    public class SalesOrderRepository : BaseRepository, ISalesOrderRepository
    {
        public SalesOrderRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [Temp_ID]
                                  ,[EmpID]
                                  ,[sBarcode]
                                  ,[Barcode]
                                  ,[ItemName]
                                  ,[SupID]
                                  ,[QTY]
                                  ,[CPU]
                                  ,[RPU]
                                  ,[VatPrcnt]
                                  ,[CREATE_BY]
                                  ,[CREATE_DATE]
                                  ,[EXPIRE_DATE]
                                  ,[CounterId]
                                  ,[IsGift]
                                  ,[SalesStockType]                             
                            ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY EmpID)  =1)
	                         THEN	(SELECT COUNT(*) FROM TempSalesOrder) 
	                         ELSE 0 END RecordCount
                    from TempSalesOrder";
        }

        public List<SALES_ORDER> GetPendingPrintItems(string stockType)
        {

            string query = @"SELECT DISTINCT SO_NO FROM dbo.SALES_ORDER WHERE IS_Print=0 AND IsCanceld=0  AND IS_APPROVED=0 and SalesStockType='" + stockType + "' ";

            var response = _dal.Select<SALES_ORDER>(query, ref msg).ToList();

            return response;
        }

        public List<SALES_ORDER> GetOrderDetails(string so_no)
        {
            string query = @"SP_ReportSOChallan '" + so_no + "' ";

            var response = _dal.Select<SALES_ORDER>(query, ref msg);
            List<SALES_ORDER> orderList = new();

            foreach (var data in response)
            {
                SALES_ORDER so = new();
                so.SO_NO = data.SO_NO;
                so.SO_ID = data.SO_ID;
                so.EmpID = data.EmpID;
                so.sBarcode = data.sBarcode;
                so.Barcode = data.Barcode;
                so.SupID = data.SupID;
                so.QTY = data.QTY;
                so.CPU = data.CPU;
                so.RPU = data.RPU;
                so.TOTAL_AMT = data.TOTAL_AMT;
                so.VAT_PRD = data.VAT_PRD;
                so.VAT = data.VAT;

                so.NET_AMT = data.NET_AMT;
                so.CREATE_BY = data.CREATE_BY;
                so.CREATE_DATE = data.CREATE_DATE;
                so.EXPIRE_DATE = data.EXPIRE_DATE;
                so.IS_APPROVED = data.IS_APPROVED;
                so.CounterId = data.CounterId;
                so.IsGift = data.IsGift;
                so.IsCanceld = data.IsCanceld;
                so.ItemName = data.ItemName;
                so.EmpName = data.Name;
                so.RFCardNo = data.RFCardNo;


                orderList.Add(so);
            }


            return orderList;
        }


        public BuyWorker GetProductInfoByBarcodeBuyWorker(string barcode, string supplierId)
        {
            string query = @"SP_GetShopStockWorker '" + barcode + "','" + supplierId + "'";
            var result = _dapper.Query<BuyWorker>(query).FirstOrDefault();
            BuyWorker buyWorker = new();
            if (result != null)
            {
                buyWorker.balQty = result.balQty;
                buyWorker.BarCode = result.BarCode;
                buyWorker.Chln = result.Chln;
                buyWorker.CMPIDX = result.CMPIDX;
                buyWorker.CPU = result.CPU;
                buyWorker.ProductDescription = result.ItemName;
                buyWorker.RPU = result.RPU;
                buyWorker.sBarCode = result.sBarCode;
                buyWorker.ShopID = result.ShopID;
                buyWorker.SupID = result.SupID;
                buyWorker.SupName = result.SupName;
                buyWorker.SupRef = result.SupRef;
                buyWorker.Transfer = result.Transfer;
                buyWorker.UserID = result.UserID;
                if (result.VATPrcnt.ToString() != "")
                    buyWorker.VATPrcnt = result.VATPrcnt;
                buyWorker.ZoneID = result.ZoneID;
                buyWorker.BarCodeExp = result.BarCodeExp;
                if (result.BoxSize.ToString() != "")
                    buyWorker.BoxSize = result.BoxSize;
                buyWorker.BoxUOMName = result.BoxUOMName;
                buyWorker.UOMName = result.UOMName;
                buyWorker.IsConversionItem = result.IsConverationItem;
                if (result.ItemWeight.ToString() != "")
                    buyWorker.ItemWeight = result.ItemWeight;
            }
            return buyWorker;

        }

        public BuyStaff GetProductInfoByBarcodeBuyStaff(string barcode, string supplierId)
        {
            string query = @"SP_GetShopStockStaff '" + barcode + "','" + supplierId + "'";
            var result = _dal.Select<BuyStaff>(query, ref msg).FirstOrDefault();

            BuyStaff BuyStaff = null;
            if (result != null)
            {
                BuyStaff = new BuyStaff();
                BuyStaff.balQty = result.balQty;
                BuyStaff.BarCode = result.BarCode;
                BuyStaff.Chln = result.Chln;
                BuyStaff.CMPIDX = result.CMPIDX;
                BuyStaff.CPU = result.CPU;
                BuyStaff.ProductDescription = string.Format("{0} , {1} ({2})",
                     result.PrdName, result.ItemName, result.UOMName);
                BuyStaff.RPU = result.RPU;
                BuyStaff.sBarCode = result.sBarCode;
                BuyStaff.ShopID = result.ShopID;
                BuyStaff.SupID = result.SupID;
                BuyStaff.SupName = result.SupName;
                BuyStaff.SupRef = result.SupRef;
                BuyStaff.Transfer = result.Transfer;
                BuyStaff.UserID = result.UserID;
                if (result.VATPrcnt != 0)
                    BuyStaff.VATPrcnt = result.VATPrcnt;

                BuyStaff.ZoneID = result.ZoneID;
                BuyStaff.BarCodeExp = result.BarCodeExp;

                if (result.BoxSize != 0)
                    BuyStaff.BoxSize = result.BoxSize;

                BuyStaff.BoxUOMName = result.BoxUOMName;
                BuyStaff.UOMName = result.UOMName;

                BuyStaff.IsConversionItem = result.IsConverationItem;
                if (result.ItemWeight != 0)
                    BuyStaff.ItemWeight = result.ItemWeight;


            }

            return BuyStaff;
        }

        public Buy GetProductInfoByBarcode(string barcode, string supplierId)
        {
            string query = @"SP_GetShopStock '" + barcode + "','" + supplierId + "'";
            var result = _dal.Select<Buy>(query, ref msg).FirstOrDefault();




            Buy buy = null;
            if (result != null)
            {
                buy = new Buy();
                buy.balQty = result.balQty;
                buy.BarCode = result.BarCode;
                buy.Chln = result.Chln;
                buy.CMPIDX = result.CMPIDX;
                buy.CPU = result.CPU;
                buy.ProductDescription = string.Format("{0} , {1} ({2})",
                     result.PrdName, result.ItemName, result.UOMName);
                buy.RPU = result.RPU;
                buy.sBarCode = result.sBarCode;
                buy.ShopID = result.ShopID;
                buy.SupID = result.SupID;
                buy.SupName = result.SupName;
                buy.SupRef = result.SupRef;
                buy.Transfer = result.Transfer;
                buy.UserID = result.UserID;
                if (result.VATPrcnt != 0)
                    buy.VATPrcnt = result.VATPrcnt;

                buy.ZoneID = result.ZoneID;
                buy.BarCodeExp = result.BarCodeExp;

                if (result.BoxSize != 0)
                    buy.BoxSize = result.BoxSize;

                buy.BoxUOMName = result.BoxUOMName;
                buy.UOMName = result.UOMName;

                buy.IsConversionItem = result.IsConverationItem;
                if (result.ItemWeight != 0)
                    buy.ItemWeight = result.ItemWeight;


            }

            return buy;
        }

        public List<SALES_ORDER> GetPendingItemsByEmp(string empId, bool isAccApproved)
        {
            string ap = isAccApproved == true ? "1" : "0";

            string query = @"SELECT DISTINCT SO_NO,SalesStockType FROM dbo.SALES_ORDER
	                        WHERE IS_APPROVED=0 AND IsCanceld=0 and IS_Print=1  and EmpID=" + empId;
            var result = _dal.Select<SALES_ORDER>(query, ref msg);
            return result;
        }
    }
}
