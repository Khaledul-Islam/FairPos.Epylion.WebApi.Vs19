using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FairPos.Epyllion.Repository.Setups;
using FairPos.Epyllion.Repository.Transfer;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Sales
{
    public interface ISalesWorkerRepository
    {
        List<TempSalesOrder> TempSalesOrderList(string UserId, string CounterId, string stocktype);
        TempSalesOrder TempSalesOrder(string UserId, string CounterId, string stocktype);
        Employee GetEmployeeById(string empId);
        Employee GetEmployeeByRFId(string rfid);
        List<Ssummary> GetInvoiceByEmployee(string empId);
        List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "All");
        bool TempSalesOrderSave(Buy buy, out string errMsg);
        bool TempSalesOrderDelete(TempSalesOrder temp, out string errMsg);
        string SalesOrderSave(List<TempSalesOrder> temp, out string errMsg);
    }

    public class SalesWorkerRepository : BaseRepository, ISalesWorkerRepository
    {
        private readonly IWorkerToMainStaffRepository _workerRepo;
        private readonly IEmployeeProductRepository _empRepo;
        public SalesWorkerRepository(IDBConnectionProvider dBConnectionProvider, IWorkerToMainStaffRepository workerRepo, IEmployeeProductRepository empRepo) : base(dBConnectionProvider)
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
            _workerRepo = workerRepo;
            _empRepo = empRepo;
        }
        private Creditlimit GetCreditLimit(string cusId)
        {
            string query = @"SP_Get_CreditLmit '" + cusId + "' ";
            var obj = _dal.Select<Creditlimit>(query, ref msg).FirstOrDefault();
            return obj;
        }
        private GetBalance GetBalance(string cusId)
        {
            string query = @"SP_Get_Balance '" + cusId + "' ";
            var obj = _dal.Select<GetBalance>(query, ref msg).FirstOrDefault();
            return obj;
        }
        public Employee GetEmployeeById(string empId)
        {
            string query = "select * from Employee where EmpID='" + empId + "' and IsActive=1";
            var data = _dal.Select<Employee>(query, ref msg).FirstOrDefault();
            if (data != null)
            {
                var limit = GetCreditLimit(data.EmpID.ToString());
                data.CreditLimit = limit.CreditLimit;
                data.AvailableLimit = limit.AvailableLimit;

                var dataImg = _dal.Select<EmployeeImage>("select * from EmployeeImage where EmpID='" + data.EmpID + "'", ref msg).FirstOrDefault();
                if (dataImg != null)
                {
                    data.EmpImage = dataImg.EmpImage;
                }
            }
            return data;
        }

        public List<TempSalesOrder> TempSalesOrderList(string UserId, string CounterId, string stocktype)
        {
            string query = "select * from TempSalesOrder where CREATE_BY='" + UserId + "' and CounterId='" + CounterId + "' and SalesStockType='" + stocktype + "'";
            var response = _dal.Select<TempSalesOrder>(query, ref msg).ToList();
            return response;
        }
        public TempSalesOrder TempSalesOrder(string UserId, string CounterId, string stocktype)
        {
            string query = "select * from TempSalesOrder where CREATE_BY='" + UserId + "' and CounterId='" + CounterId + "' and SalesStockType='" + stocktype + "'";
            var response = _dal.Select<TempSalesOrder>(query, ref msg).FirstOrDefault();
            return response;
        }
        public List<TempSalesOrder> AllTempSalesOrder()
        {
            string query = "select * from TempSalesOrder";
            var response = _dal.Select<TempSalesOrder>(query, ref msg).ToList();
            return response;
        }
        public decimal GetThisMonthConsume(string empId, string prdouctId)
        {
            string fromDate = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
            int lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            string toDay = DateTime.Now.Month.ToString() + "/" + lastDay.ToString() + "/" + DateTime.Now.Year.ToString();

            string sql = @"
                        SELECT ss.PrdID ,
                              SUM(ss.SOQTY) SOQTY,ss.EmpID FROM (

                        SELECT        StyleSize.PrdID, SUM(TempSalesOrder.QTY*ItemWeight) AS SOQTY,EmpID
                        FROM            TempSalesOrder INNER JOIN
                                                 StyleSize ON TempSalesOrder.sBarcode = StyleSize.sBarcode
						                         GROUP BY StyleSize.PrdID,EmpID
                        UNION ALL
                        SELECT        StyleSize.PrdID,SUM(SALES_ORDER.QTY*ItemWeight) AS SOQTY,EmpID
                        FROM            SALES_ORDER INNER JOIN
                                                 StyleSize ON SALES_ORDER.sBarcode = StyleSize.sBarcode
                        WHERE        (SALES_ORDER.IsCanceld = 0) AND CREATE_DATE BETWEEN '" + fromDate + "' AND '" + toDay + @"'
                        GROUP BY StyleSize.PrdID,EmpID
                        ) AS ss
                        WHERE ss.PrdID='" + prdouctId + "' AND ss.EmpID='" + empId + @"'
                        GROUP BY ss.PrdID,ss.EmpID
                        ";

            var response = _dal.Select<MonthConsume>(sql, ref msg);
            if (response != null && response.Count > 0)
            {
                decimal qty;
                decimal.TryParse(response.First().SOQTY.ToString(), out qty);
                return qty;
            }
            else
            {
                return 0;
            }
        }

        public Employee GetEmployeeByRFId(string rfid)
        {
            string query = "select * from Employee where RFCardNo='" + rfid + "'";
            var data = _dal.Select<Employee>(query, ref msg).FirstOrDefault();
            if (data != null)
            {
                if (data.StaffType == "Worker" || data.StaffType == "Non Management Staff" || data.StaffType == "Management Staff") // Management Staff added later as requirement change
                {
                    var limit = GetCreditLimit(data.EmpID.ToString());
                    data.CreditLimit = limit.CreditLimit;
                    data.AvailableLimit = limit.AvailableLimit;
                }
                else
                {
                    var balance = GetBalance(data.EmpID.ToString());
                    data.Balance = balance.Balance;
                    data.AvailableBalance = balance.AvaliableBalance;
                }

                var dataImg = _dal.Select<EmployeeImage>("select * from EmployeeImage where EmpID='" + data.EmpID + "'", ref msg).FirstOrDefault();
                if (dataImg != null)
                {
                    string EmpImage = Convert.ToBase64String(dataImg.EmpImage);
                    if (dataImg != null)
                    {
                        data.EmpImage = EmpImage;
                    }
                }
               

            }
            return data;
        }

        public List<Ssummary> GetInvoiceByEmployee(string empId)
        {
            DateTime todate = DateTime.Now.Date;
            var firstDayOfMonth = new DateTime(todate.Year, todate.Month, 1);
            string query = "select *, CAST(SaleDT as date) as SaleDTT  from Ssummary where EmpID='" + empId + "' ";
            var data = _dal.Select<Ssummary>(query, ref msg).Where(o => (o.SaleDTT >= firstDayOfMonth) &&
                             (o.SaleDTT <= todate)).ToList();
            return data;
        }

        public List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "N")
        {
            string query = @"SP_GetStockByOrgBarcodeWorker '" + PrdId + "', '" + isconversionItem + "','All' ";
            var response = _dal.Select<SupportBuy>(query, ref msg);
            List<Buy> buyList = new();
            if (response == null)
            {
                return null;
            }
            if (response.Count > 0)
            {
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
                    buy.ProductDescription = item.ItemName + "," + item.UOMName;
                    buy.RPU = item.RPU;
                    buy.AutoSale = item.AutoSale;
                    buy.ProductDescriptionBangla = item.ItemNameBangla;
                    buy.BoxUOMName = item.BoxUOMName;
                    buy.BoxSize = item.BoxSize;
                    buy.SaleBalQty = buy.balQty / buy.BoxSize;
                    buy.ItemWeight = item.ItemWeight;
                    buy.ShopID = item.ShopID;
                    buyList.Add(buy);
                }

            }

            return buyList;
        }
        private List<GiftStock> getGiftStock(string sbarcode)
        {
            string query = "select * from GiftStock where Barcode='" + sbarcode + "'";
            return _dal.Select<GiftStock>(query, ref msg).ToList();
        }
        public bool TempSalesOrderSave(Buy buy, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            decimal qty = 1;
            if (buy.AutoSale == false)
            {
                //qty = frm.qty; //ekhane qty er ekta popup ese qty input dite hoy
            }
            if (qty == 0)
            {
                errMsg = "Qty is invalid !!!";
                return false;
            }
            var items = _workerRepo.GetWorkerStaffNewBarcode(buy.sBarCode, qty);

            if (items == null || items.Count == 0)
            {
                errMsg = "Invalid Item or stock not avaialble";
                return false;
            }
            foreach (var d in items)
            {
                d.sQty = d.sQty / buy.BoxSize;
            }

            // TempSalesOrder tempData = _serviceTempOrder.Gets().Where(m => m.EmpID == _SelectedEmp.EmpID && m.CounterId != StaticData.CounterId && m.SalesStockType == _stockType).FirstOrDefault();
            TempSalesOrder tempData = AllTempSalesOrder().Where(m => m.EmpID == buy.EmpID && m.CounterId != buy.CounterID && m.SalesStockType == "Worker Stock").FirstOrDefault();
            if (tempData != null)
            {
                errMsg = "Another Counter (" + tempData.CounterId + ") already processing this Employee data";
                return false;
            }
            EmployeeProduct empItem = _empRepo.GetEmployeeProductItem(buy.EmpID.ToString()).Where(m => m.PrdID == buy.PrdID).FirstOrDefault();

            decimal thisMonthConsume = GetThisMonthConsume(buy.EmpID.ToString(), buy.PrdID);
            decimal SpousethisMonthConsume = 0;

            if (buy.SpouseId > 0)
            {
                SpousethisMonthConsume = GetThisMonthConsume(buy.SpouseId.ToString(), buy.PrdID);
            }
            thisMonthConsume = thisMonthConsume + SpousethisMonthConsume;

            if (empItem != null)
            {
                if (empItem.LimitQty > 0)
                {
                    if ((thisMonthConsume + (qty * buy.ItemWeight)) > empItem.LimitQty)
                    {
                        errMsg = "This item consume limit is over for current month, try again later !";
                        return false;
                    }
                }
            }

            decimal amountNow = 0;

            foreach (var d in items)
            {
                decimal vat = (((d.sQty * d.RPU) * d.VATPrcnt) / 100);
                vat = decimal.Round(vat, 2);
                amountNow += vat + (d.sQty * d.RPU);
            }

            // Counter counter = new CounterService(idbFactory2).GetCounerByMacAll(GlobalClass.GetMacAddressAll());
            foreach (var item in items)
            {
                string cols = "EmpID,sBarcode,Barcode,ItemName,SupID,+QTY,CPU,RPU,VatPrcnt,CREATE_BY,CREATE_DATE,EXPIRE_DATE,CounterId,IsGift,SalesStockType,ShopID";
                TempSalesOrder tso = new();

                tso.Barcode = item.BarCode;
                tso.CounterId = buy.CounterID;
                tso.CPU = item.CPU;
                tso.CREATE_BY = buy.UserID;
                tso.CREATE_DATE = DateTime.Now.Date;
                tso.EXPIRE_DATE = DateTime.Now.Date.AddMonths(2);
                tso.EmpID = buy.EmpID;
                tso.ItemName = item.ProductDescriptionBangla;
                tso.QTY = item.sQty;
                tso.RPU = item.RPU;
                tso.sBarcode = item.sBarCode;
                tso.ShopID = buy.ShopID;
                tso.SupID = item.SupID;
                tso.VatPrcnt = item.VATPrcnt;
                tso.IsGift = false;
                tso.SalesStockType = "Worker Stock";

                composite.AddRecordSet<TempSalesOrder>(tso, OperationMode.InsertOrUpdaet, "Temp_ID", cols, "sBarcode,CREATE_BY,IsGift", "");

                GiftStock gs = getGiftStock(buy.BarCode).FirstOrDefault();

                if (gs != null)
                {
                    decimal giftQty = decimal.Parse((gs.GIFT_RATIO * buy.sQty).ToString("0"));
                    if (gs.BalQty < giftQty)
                    {
                        errMsg = "Gift stock not available";
                        return false;
                    }

                    tso = new();
                    tso.Barcode = item.BarCode;
                    tso.CounterId = buy.CounterID;
                    tso.CPU = 0;
                    tso.CREATE_BY = buy.UserID;
                    tso.CREATE_DATE = DateTime.Now.Date;
                    tso.EmpID = buy.EmpID;
                    tso.EXPIRE_DATE = item.EXPDT;
                    tso.ItemName = "*" + gs.Name;
                    tso.QTY = giftQty;
                    tso.RPU = 0;
                    tso.ShopID = buy.ShopID;
                    tso.sBarcode = gs.sBarcode;
                    tso.SupID = gs.SupID;
                    tso.VatPrcnt = 0;
                    tso.IsGift = true;
                    tso.SalesStockType = "Worker Stock";
                    composite.AddRecordSet<TempSalesOrder>(tso, OperationMode.InsertOrUpdaet, "Temp_ID", cols, "sBarcode,CREATE_BY,IsGift", "");

                }
            }


            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false)
            {
                errMsg = msg;
            }
            return response;


        }

        public bool TempSalesOrderDelete(TempSalesOrder temp, out string errMsg)
        {
            errMsg = string.Empty;
            var response  = _dal.Delete<TempSalesOrder>(temp, "CREATE_BY,sBarcode,Barcode", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
            }
            return response;
        }

        public string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                decimal availLimit = olist.First().AvailableLimit;
                decimal currentAmnt = olist.First().GrandTotal;
                string year = DateTime.Now.Year.ToString() + olist.First().CounterId;
                string so_no = GetMaxIdWithPrfix2("SO_NO", "000000", "000001", "SALES_ORDER", year);
                decimal totalVat = 0;
                foreach (var item in olist)
                {
                    decimal vatAmnt = ((item.QTY * item.RPU) * item.VatPrcnt) / 100;
                    vatAmnt = decimal.Round(vatAmnt, 2);
                    totalVat += vatAmnt;
                }
                totalVat = Math.Ceiling(totalVat);
                int pos = 1;
                decimal incVat = 0;
                decimal incTotalAmt = 0;
                CompositeModel composite = new();
                foreach (TempSalesOrder item in olist)
                {
                    SALES_ORDER so = new();
                    so.Barcode = item.Barcode;
                    so.CounterId = item.CounterId;
                    so.CPU = item.CPU;
                    so.CREATE_BY = item.CREATE_BY;
                    so.CREATE_DATE = DateTime.Now.Date;
                    so.EmpID = item.EmpID;
                    so.ShopID = item.ShopID;
                    so.EXPIRE_DATE = item.EXPIRE_DATE;
                    so.IsAccountApproved = true;
                    so.QTY = item.QTY;
                    so.RPU = item.RPU;
                    so.sBarcode = item.sBarcode;
                    so.SupID = item.SupID;
                    so.VAT_PRD = item.VatPrcnt;
                    so.IsGift = item.IsGift;
                    so.SalesStockType = "Worker Stock";
                    so.TOTAL_AMT = decimal.Round((((item.QTY * item.RPU) * item.VatPrcnt) / 100), 2) + (item.QTY * item.RPU);
                    so.VAT = totalVat;
                    so.VAT_PRD = decimal.Round((((item.QTY * item.RPU) * item.VatPrcnt) / 100), 2);
                    so.NET_AMT = currentAmnt;
                    so.SO_NO = so_no;
                    so.IS_APPROVED = false;
                    so.IS_Print = false;
                    so.IsCanceld = false;
                    #region adjust extra fractional value
                    incTotalAmt += so.TOTAL_AMT;
                    incVat += so.VAT_PRD;
                    if (pos > 1 && pos == olist.Count)
                    {
                        if (incTotalAmt > currentAmnt)
                            so.TOTAL_AMT = so.TOTAL_AMT - (incTotalAmt - currentAmnt);
                        else if (incTotalAmt < currentAmnt)
                            so.TOTAL_AMT = so.TOTAL_AMT + (currentAmnt - incTotalAmt);

                        if (incVat > totalVat)
                            so.VAT_PRD = so.VAT_PRD - (incVat - totalVat);
                        else if (incVat < totalVat)
                            so.VAT_PRD = so.VAT_PRD + (totalVat - incVat);
                    }

                    pos++;
                    #endregion
                    composite.AddRecordSet<SALES_ORDER>(so, OperationMode.Insert, "SO_ID", "", "", "");
                }
                composite.AddRecordSet<TempSalesOrder>(olist.First(), OperationMode.Delete, "SO_ID", "", "CREATE_BY,CounterId", "");

                var response = _dal.InsertUpdateComposite(composite, ref msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                return so_no;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
