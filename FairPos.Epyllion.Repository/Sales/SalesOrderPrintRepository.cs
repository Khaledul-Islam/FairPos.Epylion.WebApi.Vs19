using Dapper;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Sales
{
    public interface ISalesOrderPrintRepository
    {
        List<string> PendingSalesOrder();
        List<string> PendingSalesOrderStaff();
        bool IS_SO_PRINTED(string so_no);
        string SalesOrderDeliverySave(List<SALES_ORDER> oList);
        string SalesOrderDeliverySaveStaff(List<SALES_ORDER> oList);
    }
    public class SalesOrderPrintRepository : BaseRepository, ISalesOrderPrintRepository
    {
        public SalesOrderPrintRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
        }

        public bool IS_SO_PRINTED(string so_no)
        {
            SALES_ORDER sales = new();
            sales.IS_Print = true;
            sales.SO_NO = so_no;
            var response = _dal.Update<SALES_ORDER>(sales, "IS_Print", "SO_NO", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return false;
            }
            return response;
        }

        public List<string> PendingSalesOrder()
        {
            string query = @"SELECT DISTINCT SO_NO FROM dbo.SALES_ORDER
	                        WHERE IS_Print=0 AND IsCanceld=0  AND IS_APPROVED=0 and SalesStockType='Worker Stock' ";
            //var respoense = _dal.Select<String>(query, ref msg).ToList();
            var response = _dapper.Query<string>(query).ToList();
            return response;
        }
        public List<string> PendingSalesOrderStaff()
        {
            string query = @"SELECT DISTINCT SO_NO FROM dbo.SALES_ORDER
	                        WHERE IS_Print=0 AND IsCanceld=0  AND IS_APPROVED=0 and SalesStockType='Staff Stock' ";
            //var response = _dal.Select<String>(query, ref msg).ToList();
            var response = _dapper.Query<String>(query).ToList();
            return response;
        }

        public string SalesOrderDeliverySave(List<SALES_ORDER> oList)
        {
            try
            {
                int nofItem = 0;
                string prefix = string.Format("{0}{1}{2}{3}{4}",
                                                    DateTime.Now.Month.ToString("00"),
                                                    DateTime.Now.Day.ToString("00"),
                                                    DateTime.Now.Year.ToString().Substring(2, 2),
                                                    oList.First().EmpID,
                                                     oList.First().CounterName);
                string invoieNO = prefix + GetMaxId("EmpID", "Ssummary");
                decimal _totalAmnt = 0;
                decimal totalCost = 0;
                decimal _rtnAmnt = 0;
                decimal _rtnCost = 0;
                foreach (var s in oList)
                {
                    totalCost += (s.QTY * s.CPU);
                    _totalAmnt += s.TOTAL_AMT;
                    if (s.IsGift == false)
                        nofItem++;
                }
                int count = 1;
                string packageNo = "";
                CompositeModel composite = new();
                foreach (var s in oList)
                {
                    Sale sale = new();
                    sale.SaleDT = DateTime.Now;
                    sale.CmpIDX = invoieNO + s.Barcode;
                    if (s.IsGift == true)
                        sale.CmpIDX += "x";
                    sale.SupID = s.SupID;
                    sale.Qty = 0;
                    sale.CPU = s.CPU;
                    sale.RPP = 0;
                    sale.RPU = s.RPU;
                    sale.VPP = 0;
                    sale.VPU = 0;
                    sale.WSP = 0;
                    sale.WSQ = 0;
                    sale.sBarCode = s.sBarcode;
                    sale.BarCode = s.Barcode;
                    sale.SQty = s.QTY;
                    sale.rQty = 0;
                    sale.rAmt = 0;
                    sale.rVat = 0;
                    sale.rDisc = 0;
                    sale.IsCircularDiscount = "N";
                    sale.IsGift = s.IsGift;
                    sale.SalesStockType = "Worker Stock";
                    sale.cInvoice = "";
                    sale.ShopID = oList.First().ShopID;
                    sale.PayType = "Full-Credit";

                    #region discount set
                    {
                        sale.DiscPrcnt = 0;
                        sale.DiscAmtPrd = 0;
                    }
                    sale.DiscAmt = 0;
                    #endregion

                    #region vat amount set

                    {
                        sale.VATPrd = s.VAT_PRD;
                        sale.VATPrcnt = 0;
                        sale.VAT = s.VAT;
                    }
                    #endregion

                    {
                        sale.TotalAmt = s.TOTAL_AMT;
                        sale.TotalCost = (sale.SQty * sale.CPU);
                    }
                    sale.NetAmt = s.NET_AMT;
                    sale.PrdSlNo = "";
                    sale.CshAmt = 0;
                    sale.CrdAmt = 0;
                    sale.Salesman = "0000";
                    sale.Invoice = invoieNO;
                    sale.CardName = "";
                    sale.CardNo = "";
                    sale.CounterID = oList.First().CounterId;
                    sale.PrvCusID = "";
                    sale.CusName = "";
                    sale.Posted = "N";
                    sale.Returned = "N";
                    sale.ReturnedType = "N";
                    sale.ReturnedDt = DateTime.Parse("01/01/1900");
                    sale.Flag = "N";
                    sale.Point = 0;
                    sale.UserId = s.CREATE_BY;
                    sale.EmpID = s.EmpID;
                    sale.DelveryNo = s.SO_NO;

                    composite.AddRecordSet<Sale>(sale, OperationMode.Insert, "", "", "", "");

                    if (sale.IsGift == false)
                    {
                        var data = _dal.Select<BuyWorker>("select * from BuyWorker where BarCode='" + sale.BarCode + "'", ref msg).FirstOrDefault();
                        var dataSS = _dal.Select<StyleSize>("select * from StyleSize where sBarcode='" + data.sBarCode + "'", ref msg).FirstOrDefault();
                        data.Transfer = "N";
                        data.balQty -= (sale.SQty * dataSS.BoxSize);
                        data.sQty += (sale.SQty * dataSS.BoxSize);
                        composite.AddRecordSet<BuyWorker>(data, OperationMode.InsertOrUpdaet, "", " Transfer,balQty,sQty", "BarCode", "");
                    }
                    else
                    {
                        var data = _dal.Select<GiftStock>("select * from GiftStock where Barcode= '" + sale.BarCode + "'", ref msg).FirstOrDefault();
                        data.BalQty -= sale.SQty;
                        data.SQty += sale.SQty;
                        composite.AddRecordSet<GiftStock>(data, OperationMode.InsertOrUpdaet, "", " BalQty, SQty", "Barcode", "");
                    }

                    count++;
                }
                Ssummary summary = new();
                summary.SaleDT = DateTime.Now;
                summary.Invoice = invoieNO;
                summary.TotalCost = totalCost;
                summary.TotalAmt = _totalAmnt;
                summary.Discount = 0;
                summary.DiscAmt = 0;
                summary.VAT = oList[0].VAT;
                summary.NetAmt = oList[0].NET_AMT;
                summary.RoundValue = 0;
                summary.CshAmt = 0;
                summary.CrdAmt = 0;
                summary.PayType = "Full-Credit";
                summary.SalesStockType = "Worker Stock";
                summary.Salesman = "0000";
                summary.ShopID = oList.First().ShopID;
                summary.CounterID = oList.First().CounterId;
                summary.CardName = "";
                summary.CardNo = "";
                summary.PrvCus = "T";
                summary.CusName = "";
                summary.PrvCusID = "";
                summary.ReturnedAmt = _rtnAmnt;
                summary.rTotalCost = _rtnCost;
                summary.cInvoice = "";
                summary.ReturnedType = "N";
                summary.Flag = "N";
                summary.PaidAmt = 0;
                summary.ChangeAmt = 0;
                summary.Point = 0;
                summary.PackageNo = packageNo;
                summary.UserId = oList.First().CREATE_BY;
                summary.EmpID = oList.First().EmpID.ToString();
                summary.DelveryNo = oList.First().SO_NO;

                composite.AddRecordSet<Ssummary>(summary, OperationMode.Insert, "", "", "", "");
                SALES_ORDER dataPrv = new();
                dataPrv.IS_APPROVED = true;
                dataPrv.DeliverdCancelBy = summary.UserId;
                dataPrv.DeliverDate = DateTime.Now;
                dataPrv.SO_NO = oList.First().SO_NO;
                composite.AddRecordSet<SALES_ORDER>(dataPrv, OperationMode.Update, " ", "IS_APPROVED,DeliverdCancelBy,DeliverDate", "SO_NO", "");
                var response = _dal.InsertUpdateComposite(composite, ref msg);
                if (response)
                {
                    return invoieNO;
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

        public string SalesOrderDeliverySaveStaff(List<SALES_ORDER> oList)
        {
            try
            {
                int nofItem = 0;
                string prefix = string.Format("{0}{1}{2}{3}{4}",
                                                    DateTime.Now.Month.ToString("00"),
                                                    DateTime.Now.Day.ToString("00"),
                                                    DateTime.Now.Year.ToString().Substring(2, 2),
                                                    oList.First().EmpID,
                                                     oList.First().CounterName);
                string invoieNO = prefix + GetMaxId("EmpID", "Ssummary");
                decimal _totalAmnt = 0;
                decimal totalCost = 0;
                decimal _rtnAmnt = 0;
                decimal _rtnCost = 0;
                foreach (var s in oList)
                {
                    totalCost += (s.QTY * s.CPU);
                    _totalAmnt += s.TOTAL_AMT;
                    if (s.IsGift == false)
                        nofItem++;
                }
                int count = 1;
                string packageNo = "";
                CompositeModel composite = new();
                foreach (var s in oList)
                {
                    Sale sale = new();
                    sale.SaleDT = DateTime.Now;
                    sale.CmpIDX = invoieNO + s.Barcode;
                    if (s.IsGift == true)
                        sale.CmpIDX += "x";
                    sale.SupID = s.SupID;
                    sale.Qty = 0;
                    sale.CPU = s.CPU;
                    sale.RPP = 0;
                    sale.RPU = s.RPU;
                    sale.VPP = 0;
                    sale.VPU = 0;
                    sale.WSP = 0;
                    sale.WSQ = 0;
                    sale.sBarCode = s.sBarcode;
                    sale.BarCode = s.Barcode;
                    sale.SQty = s.QTY;
                    sale.rQty = 0;
                    sale.rAmt = 0;
                    sale.rVat = 0;
                    sale.rDisc = 0;
                    sale.IsCircularDiscount = "N";
                    sale.IsGift = s.IsGift;
                    sale.SalesStockType = "Staff Stock";
                    sale.cInvoice = "";
                    sale.ShopID = oList.First().ShopID;
                    sale.PayType = "Full-Credit";

                    #region discount set
                    {
                        sale.DiscPrcnt = 0;
                        sale.DiscAmtPrd = 0;
                    }
                    sale.DiscAmt = 0;
                    #endregion

                    #region vat amount set

                    {
                        sale.VATPrd = s.VAT_PRD;
                        sale.VATPrcnt = 0;
                        sale.VAT = s.VAT;
                    }
                    #endregion

                    {
                        sale.TotalAmt = s.TOTAL_AMT;
                        sale.TotalCost = (sale.SQty * sale.CPU);
                    }
                    sale.NetAmt = s.NET_AMT;
                    sale.PrdSlNo = "";
                    sale.CshAmt = 0;
                    sale.CrdAmt = 0;
                    sale.Salesman = "0000";
                    sale.Invoice = invoieNO;
                    sale.CardName = "";
                    sale.CardNo = "";
                    sale.CounterID = oList.First().CounterId;
                    sale.PrvCusID = "";
                    sale.CusName = "";
                    sale.Posted = "N";
                    sale.Returned = "N";
                    sale.ReturnedType = "N";
                    sale.ReturnedDt = DateTime.Parse("01/01/1900");
                    sale.Flag = "N";
                    sale.Point = 0;
                    sale.UserId = s.CREATE_BY;
                    sale.EmpID = s.EmpID;
                    sale.DelveryNo = s.SO_NO;

                    composite.AddRecordSet<Sale>(sale, OperationMode.Insert, "", "", "", "");

                    if (sale.IsGift == false)
                    {
                        var data = _dal.Select<BuyStaff>("select * from BuyStaff where BarCode='" + sale.BarCode + "'", ref msg).FirstOrDefault();
                        var dataSS = _dal.Select<StyleSize>("select * from StyleSize where sBarcode='" + data.sBarCode + "'", ref msg).FirstOrDefault();
                        data.Transfer = "N";
                        data.balQty -= (sale.SQty * dataSS.BoxSize);
                        data.sQty += (sale.SQty * dataSS.BoxSize);
                        composite.AddRecordSet<BuyStaff>(data, OperationMode.InsertOrUpdaet, "", " Transfer,balQty,sQty", "BarCode", "");
                    }
                    else
                    {
                        var data = _dal.Select<GiftStock>("select * from GiftStock where Barcode= '" + sale.BarCode + "'", ref msg).FirstOrDefault();
                        data.BalQty -= sale.SQty;
                        data.SQty += sale.SQty;
                        composite.AddRecordSet<GiftStock>(data, OperationMode.InsertOrUpdaet, "", " BalQty, SQty", "Barcode", "");
                    }

                    count++;
                }
                Ssummary summary = new();
                summary.SaleDT = DateTime.Now;
                summary.Invoice = invoieNO;
                summary.TotalCost = totalCost;
                summary.TotalAmt = _totalAmnt;
                summary.Discount = 0;
                summary.DiscAmt = 0;
                summary.VAT = oList[0].VAT;
                summary.NetAmt = oList[0].NET_AMT;
                summary.RoundValue = 0;
                summary.CshAmt = 0;
                summary.CrdAmt = 0;
                summary.PayType = "Full-Credit";
                summary.SalesStockType = "Staff Stock";
                summary.Salesman = "0000";
                summary.ShopID = oList.First().ShopID;
                summary.CounterID = oList.First().CounterId;
                summary.CardName = "";
                summary.CardNo = "";
                summary.PrvCus = "T";
                summary.CusName = "";
                summary.PrvCusID = "";
                summary.ReturnedAmt = _rtnAmnt;
                summary.rTotalCost = _rtnCost;
                summary.cInvoice = "";
                summary.ReturnedType = "N";
                summary.Flag = "N";
                summary.PaidAmt = 0;
                summary.ChangeAmt = 0;
                summary.Point = 0;
                summary.PackageNo = packageNo;
                summary.UserId = oList.First().CREATE_BY;
                summary.EmpID = oList.First().EmpID.ToString();
                summary.DelveryNo = oList.First().SO_NO;

                composite.AddRecordSet<Ssummary>(summary, OperationMode.Insert, "", "", "", "");
                SALES_ORDER dataPrv = new();
                dataPrv.IS_APPROVED = true;
                dataPrv.DeliverdCancelBy = summary.UserId;
                dataPrv.DeliverDate = DateTime.Now;
                dataPrv.SO_NO = oList.First().SO_NO;
                composite.AddRecordSet<SALES_ORDER>(dataPrv, OperationMode.Update, " ", "IS_APPROVED,DeliverdCancelBy,DeliverDate", "SO_NO", "");
                var response = _dal.InsertUpdateComposite(composite, ref msg);
                if (response)
                {
                    return invoieNO;
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
