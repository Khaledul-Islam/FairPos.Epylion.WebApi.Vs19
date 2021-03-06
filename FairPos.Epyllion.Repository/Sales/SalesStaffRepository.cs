using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Sales
{
    public interface ISalesStaffRepository
    {
        bool saveManagementStaffTopUp(TopUp model);
        List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "N");
        bool TempSalesOrderSave(Buy buy, out string errMsg);
        string SalesOrderSave(List<TempSalesOrder> olist, out string errMsg);        
    }

    public class SalesStaffRepository : BaseRepository, ISalesStaffRepository
    {
        private readonly IEmployeeProductRepository _empRepo;
        private readonly IEmployeeRepository _emp;
        public SalesStaffRepository(IDBConnectionProvider dBConnectionProvider, IEmployeeProductRepository empRepo, IEmployeeRepository emp) : base(dBConnectionProvider)
        {
            _empRepo = empRepo;
            _emp = emp;
        }
        private List<GiftStock> getGiftStock(string sbarcode)
        {
            string query = "select * from GiftStock where Barcode='" + sbarcode + "'";
            return _dal.Select<GiftStock>(query, ref msg).ToList();
        }
        private StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var query = "select * from vStyleSize where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }
        private List<Buy> GetWorkerStaffNewBarcode(string sBarcode, decimal qty)
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
            string query = @"SP_GetNewBarcodeBySbarocdeStaff '" + sBarcode + "' ," + qty + "";
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
                var emp = _emp.FindById(olist.First().EmpID.ToString());
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
                    so.EXPIRE_DATE = item.EXPIRE_DATE;
                    so.IsAccountApproved = true;
                    if (emp != null)
                    {
                        if (emp.StaffType == "Management Staff")
                        {
                            so.IsAccountApproved = false;
                        }
                    }
                    so.QTY = item.QTY;
                    so.RPU = item.RPU;
                    so.sBarcode = item.sBarcode;
                    so.SupID = item.SupID;
                    so.ShopID = item.ShopID;
                    so.VAT_PRD = item.VatPrcnt;
                    so.IsGift = item.IsGift;
                    so.SalesStockType = "Staff Stock";
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


        public bool saveManagementStaffTopUp(TopUp model)
        {
            string prefix = DateTime.Now.ToString("yyyyMMdd");
            string collectionNo = GetMaxIdWithPrfix2("CollectionNo", "000000", "000001", "StaffLedger", prefix);

            StaffLedger ledger = new ()
            {
                CollectionNo = collectionNo,
                EmpID = model.EmpID,
                DrAmount = model.topup,
                Description =model.desc,
                CreatedBy = model.UserId,
                CreatedDate = DateTime.Now.Date
            };

            var response = _dal.Insert<StaffLedger>(ledger, "", "Id", "", ref msg);
            return response;
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
            var items = GetWorkerStaffNewBarcode(buy.sBarCode, qty);

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
                tso.ShopID = buy.ShopID;
                tso.RPU = item.RPU;
                tso.sBarcode = item.sBarCode;
                tso.SupID = item.SupID;
                tso.VatPrcnt = item.VATPrcnt;
                tso.IsGift = false;
                tso.SalesStockType = "Staff Stock";

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
                    tso.ShopID = buy.ShopID;
                    tso.EXPIRE_DATE = item.EXPDT;
                    tso.ItemName = "*" + gs.Name;
                    tso.QTY = giftQty;
                    tso.RPU = 0;
                    tso.sBarcode = gs.sBarcode;
                    tso.SupID = gs.SupID;
                    tso.VatPrcnt = 0;
                    tso.IsGift = true;
                    tso.SalesStockType = "Staff Stock";
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
        public List<Buy> GetProductsByOrgBarcode(string PrdId, string isconversionItem = "N")
        {
            string query = @"SP_GetStockByOrgBarcodeStaff '" + PrdId + "', '" + isconversionItem + "' ";
            var response = _dal.Select<SupportBuy>(query, ref msg);
            List<Buy> buyList = new();
            if(response == null)
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
                    buyList.Add(buy);
                }

            }

            return buyList;
        }
    }
}
