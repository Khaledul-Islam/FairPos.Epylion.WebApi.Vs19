using Dapper;
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
    public interface IItemConversionRepository
    {
        bool DeleteTempConversionStock(string UserID);
        List<TempConversionStock> TempConvStockList();
        List<TempConversionStock> TempConvStockListByID(string UserID);
        List<Buy> GetProductsSupplier(string SupId, string ShopID);
        Buy GetByBarcodeExp(string expbarcode);
        List<StyleSizeCoversition> GetItemDetails(string ParentSbarcode, DateTime parentItemExpire);
        List<Buy> GetNewBarcodeBySbarcode(string sBarcode, decimal qty);
        bool SaveTempConvStock(TempConversionStock model);
        string SaveConvStock(List<StyleSizeCoversition> model, out string errMsg);
    }
    public class ItemConversionRepository : BaseRepository, IItemConversionRepository
    {
        public ItemConversionRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = "";
        }
        public bool DeleteTempConversionStock(string UserID)
        {
            string query = "delete from TempConversionStock where UserName=@UserName SELECT @@ROWCOUNT";
            var response = _dapper.Query<int>(query, new
            {
                @UserName = UserID
            }).FirstOrDefault();
            if (response > 0)
            {
                return true;
            }
            return false;

        }

        public Buy GetByBarcodeExp(string expbarcode)
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

        public List<StyleSizeCoversition> GetItemDetails(string ParentSbarcode, DateTime parentItemExpire)
        {
            string query = @"SELECT StyleSizeCoversition.ConversitionId, StyleSizeCoversition.MainSBarcode, StyleSizeCoversition.MainBarcode, 
                                    StyleSizeCoversition.sBarcode, StyleSizeCoversition.Barcode, vStyleSize.ItemName, 
                                    vStyleSize.UOMName, vStyleSize.ItemFullName, vStyleSize.ItemNameBangla, vStyleSize.ItemNameBangla,
                                    vStyleSize.BoxUOMName, vStyleSize.BoxSize FROM StyleSizeCoversition INNER JOIN
                                    vStyleSize ON StyleSizeCoversition.sBarcode = vStyleSize.sBarcode
                            WHERE MainSBarcode='" + ParentSbarcode + "'";
            var response = _dapper.Query<StyleSizeCoversition>(query).ToList();
            if (response.Count <= 0)
            {
                return new List<StyleSizeCoversition>();
            }
            List<StyleSizeCoversition> buyList = new();
            foreach (var item in response)
            {
                StyleSizeCoversition itemConv = new();
                itemConv.ConversitionId = item.ConversitionId;
                itemConv.MainSBarcode = item.MainSBarcode;
                itemConv.MainBarcode = item.MainBarcode;
                itemConv.sBarcode = item.sBarcode;
                itemConv.Barcode = item.Barcode;
                itemConv.ItemName = item.ItemName;
                itemConv.UOMName = item.UOMName;
                itemConv.ItemFullName = item.ItemFullName;
                itemConv.ItemNameBangla = item.ItemNameBangla;
                itemConv.ItemFullNameBangla = item.ItemFullNameBangla;
                itemConv.BoxUOMName = item.BoxUOMName;
                itemConv.BoxSize = item.BoxSize;
                itemConv.ExpireDate = parentItemExpire;
                buyList.Add(itemConv);
            }
            return buyList;
        }
        private StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var query = "select * from vStyleSize where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        //done
        public List<Buy> GetNewBarcodeBySbarcode(string sBarcode, decimal qty)
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

        //done
        public List<Buy> GetProductsSupplier(string SupId, string ShopID)
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
                return buyList.Where(m => m.IsConversionItem == true).ToList();
            }
            List<Buy> data = buyList.Where(m => m.IsConversionItem == true && m.SupID == SupId).ToList();
            return data;
        }

        public List<TempConversionStock> TempConvStockList()
        {
            string query = "select * from TempConversionStock";
            var response = _dal.Select<TempConversionStock>(query, ref msg).ToList();
            return response;
        }
        public List<TempConversionStock> TempConvStockListByID(string UserID)
        {
            string query = "select * from TempConversionStock where UserName= '" + UserID + "'";
            var response = _dal.Select<TempConversionStock>(query, ref msg).ToList();
            return response;
        }

        public bool SaveTempConvStock(TempConversionStock model)
        {
            if (model == null)
            {
                return false;
            }
            var response = _dal.Insert<TempConversionStock>(model, "", "AutoId", "", ref msg);
            return response;
        }

        private StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            var query = "select * from vStyleSize where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        public string SaveConvStock(List<StyleSizeCoversition> model, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                List<ConversionStock> olist = new();
                string conversionNo = GetMaxIdWithPrfix2("ConversitionNo", "000000", "000001", "ConversionStock", "C");
                var SelectedItem = GetByBarcodeExp(model.First().bCode);
                foreach (var item in model)
                {
                    ConversionStock cs = new();
                    decimal boxSize = 0;
                    if (item.QTY.ToString() == null)
                    {
                        continue;
                    }

                    boxSize = item.BoxSize;
                    decimal tempQty = item.QTY;

                    if (tempQty <= 0)
                    {
                        continue;
                    }

                    cs.ConversitionNo = conversionNo;
                    cs.FromSBarcode = SelectedItem.sBarCode;
                    cs.ConDate = DateTime.Now;
                    cs.FromBarcodeExp = item.bCode;
                    cs.FromBoxQty = item.boxQTY;
                    cs.FromUnitQty = item.unitQTY;
                    cs.ToBarcodeExp = item.Barcode;//00071
                    StyleSize ss = StyleSizeDetailsByBarcode(cs.ToBarcodeExp);
                    cs.ToBoxQty = tempQty;
                    cs.ToUniQty = tempQty * boxSize;
                    cs.ToSBarcode = ss.sBarcode;
                    cs.ExpireDate = item.ExpireDate;
                    cs.ToBarcodeExp = cs.ToBarcodeExp + cs.ExpireDate.Year.ToString() + cs.ExpireDate.Month.ToString("00") + cs.ExpireDate.Day.ToString("00");

                    if (cs.ToBoxQty > 0)
                    {
                        double days = (cs.ExpireDate - DateTime.Now.Date).TotalDays;
                        if (ss.ArrivalExpireLimit.ToString() == null || Convert.ToBoolean(ss.ArrivalExpireLimit) == false)
                        {
                            errMsg = "Arrival expiry not set for this product for barocde :" + cs.ToBarcodeExp;
                            return "false";
                        }
                        if (days < ss.ArrivalExpireLimit)
                        {
                            errMsg = "Expiry date limit is not valid for barocde :" + cs.ToBarcodeExp;
                            return "false";
                        }
                    }

                    olist.Add(cs);
                }
                if (olist.Count == 0)
                {
                    errMsg = "NO item found to save";
                    return "false";
                }
                CompositeModel composite = new();
                foreach (var d in olist)
                {
                    composite.AddRecordSet<ConversionStock>(d, OperationMode.Insert, "ConversitionId", "", "", "");
                    // _dal.Insert<ConversionStock>(d, "", "ConversitionId", "", ref msg);
                    #region   insert to buy
                    StyleSize ss = StyleSizeDetailsBySbarcode(d.ToSBarcode);
                    Buy buy = new();
                    string query = $@"select * from Buy  WHERE  sBarCode='{d.ToSBarcode}' and BarCode='{d.ToBarcodeExp}' ";
                    var dataPrv = _dal.Select<Buy>(query, ref msg).FirstOrDefault();
                    buy.CMPIDX = d.ConversitionNo + d.ToSBarcode;
                    buy.sBarCode = d.ToSBarcode;
                    buy.BarCode = d.ToBarcodeExp;
                    buy.Chln = d.ConversitionNo;
                    buy.BuyDT = DateTime.Now; //ichllan.BuyDT;
                    buy.CPU = ss.CPU;
                    buy.LCPU = ss.CPU;
                    buy.RPU = ss.RPU;
                    if (dataPrv != null)
                    {
                        buy.CPU = decimal.Round((((buy.CPU * buy.Qty) + (dataPrv.balQty * dataPrv.CPU)) / (buy.Qty + dataPrv.balQty)), 2);
                        buy.RPU = decimal.Round((((buy.RPU * buy.Qty) + (dataPrv.balQty * dataPrv.RPU)) / (buy.Qty + dataPrv.balQty)), 2);
                    }
                    buy.RPP = 0;
                    buy.CQty = 0;
                    buy.WSP = 0;
                    buy.WSQ = 0;
                    buy.DiscPrcnt = ss.DiscPrcnt;
                    buy.VATPrcnt = ss.VATPrcnt;
                    buy.PrdComm = ss.PrdComm;
                    buy.Qty = d.ToUniQty;
                    buy.bQty = 0;
                    buy.TrfQty = 0;
                    buy.sreturn = 0;
                    buy.sQty = 0;
                    buy.rQty = 0;
                    buy.SrpQty = 0;
                    buy.dmlqty = 0;
                    buy.BarocdeDate = d.ExpireDate;
                    buy.InvQty = 0;
                    buy.balQty = d.ToUniQty;
                    buy.SupID = ss.SupID;
                    buy.EXPDT = d.ExpireDate;
                    buy.ShopID = model.First().ShopID;
                    buy.Transfer = "N";
                    buy.SupRef = "NA";
                    buy.UserID = model.First().UserID;
                    buy.Point = 0;
                    buy.Reorder = 0;
                    buy.ZoneID = "";

                    string col = "CMPIDX,sBarCode,BarCode,Chln,BuyDT,CPU,LCPU,RPU,RPP,WSP,WSQ,DiscPrcnt,VATPrcnt," +
                                   "PrdComm,+Qty,CQty,bQty,TrfQty,sreturn,sQty,rQty,SrpQty,dmlqty,InvQty,+balQty," +
                                   "SupID,EXPDT,ShopID,Transfer,SupRef,UserID,Point,Reorder,ZoneID,BarocdeDate," +
                                   "WriteOff,WriteOn,TransferIn,TransferOut ";
                    composite.AddRecordSet<Buy>(buy, OperationMode.InsertOrUpdaet, "", col, "BarCode,sBarCode", "");
                    #endregion
                }
                Buy b = new();
                b.BarCode = olist.First().FromBarcodeExp;
                b.CQty += model.First().totalqty;
                b.balQty += model.First().totalqty;

                composite.AddRecordSet<Buy>(b, OperationMode.Update, "", "+CQty,-balQty", "BarCode", "");
                TempConversionStock temp = new();
                temp.UserName = model.First().UserID;
                composite.AddRecordSet<TempConversionStock>(temp, OperationMode.Delete, "AutoId", "", "UserName", "");
                var response = _dal.InsertUpdateComposite(composite, ref msg);
                if (response)
                {
                    return conversionNo;
                }
                else
                {
                    errMsg = msg;
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