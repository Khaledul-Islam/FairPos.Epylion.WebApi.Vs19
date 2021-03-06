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
    public interface IInventoryRepository
    {
        Buy GetByBarcodeExpForInventory(string barcode);
        bool RemoveTempInventory(TempInventory model);
        bool SaveTempInventory(TempInventory model, out string errMsg);
        string SaveInventory(List<TempInventory> olist, out string errMsg);
        List<TempInventory> GetsTempInventory(string name);
    }

    public class InventoryRepository : BaseRepository, IInventoryRepository
    {
        public InventoryRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

        }
        public Buy GetByBarcodeExpForInventory(string barcode)
        {
            string query = @"SP_GetByBarcodeExp2 '" + barcode + "' ";
            var result = _dal.Select<SupportBuy>(query, ref msg).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            Buy buy = new();
            buy.balQty = result.BalQty;
            buy.BarCode = result.Barcode;
            buy.sBarCode = result.sBarcode;
            buy.PrdID = result.PrdID;
            buy.PrdName = result.PrdName;
            buy.SupID = result.SupID;
            buy.SupName = result.Supname;
            buy.UOMName = result.UOMName;
            buy.ProductDescription = result.ItemFullName;
            buy.ProductDescriptionBangla = result.ItemFullNameBangla;
            buy.RPU = result.RPU;
            buy.CPU = result.CPU;
            buy.VATPrcnt = result.VATPrcnt;
            buy.EXPDT = result.EXPDT;
            buy.BoxUOMName = result.BoxUOMName;
            buy.BoxSize = result.BoxSize;
            buy.IsConversionItem = result.IsConverationItem;
            buy.SaleBalQty = result.SO_Pending;

            return buy;
        }

        public List<TempInventory> GetsTempInventory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                string query = "select * from TempInventory";
                var data = _dal.Select<TempInventory>(query, ref msg);
                return data;
            }
            else
            {
                string query = "select * from TempInventory where CREATE_BY ='" + name + "'";
                var data = _dal.Select<TempInventory>(query, ref msg);
                return data;
            }
        }

        public bool RemoveTempInventory(TempInventory model)
        {
            var response = _dal.Delete<TempInventory>(model, "BarCode,CREATE_BY", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                return false;
            }
            return response;
        }

        public string SaveInventory(List<TempInventory> olist, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                if (olist == null || olist.Count == 0)
                {
                    errMsg = "No data for save";
                    return "false";
                }
                string chln = GetMaxIdWithPrfix2("InvenotryNo", "0000", "0001", "Inventory", "I/" + DateTime.Now.Year.ToString() + "/");
                CompositeModel composite = new();
                foreach (var d in olist)
                {
                    Inventory sts = new();
                    sts.BarCode = d.BarCode;
                    sts.sBarCode = d.sBarCode;
                    sts.InvenotryNo = chln;
                    sts.StockBoxQty = d.StockBoxQty;
                    sts.WriteOffBoxQty = d.WriteOffBoxQty;
                    sts.CREATE_DATE = DateTime.Now;
                    sts.WriteOnBoxQty = d.WriteOnBoxQty;
                    sts.CREATE_BY = d.UserID;
                    sts.BoxSize = d.BoxSize;
                    sts.RPU = d.RPU;
                    composite.AddRecordSet<Inventory>(sts, OperationMode.Insert, "InventoryId", "", "", "");
                    if (sts.WriteOffBoxQty > 0)
                    {
                        Buy buy = new();
                        buy.BarCode = sts.BarCode;
                        buy.ShopID = d.ShopId;
                        buy.Transfer = "N";
                        buy.balQty += (sts.WriteOffBoxQty * sts.BoxSize);
                        buy.WriteOff += (sts.WriteOffBoxQty * sts.BoxSize);
                        composite.AddRecordSet<Buy>(buy, OperationMode.Update, "", "Transfer,-balQty,+WriteOff", "BarCode,ShopID", "");
                    }
                    else if (sts.WriteOnBoxQty > 0)
                    {
                        Buy buy = new();
                        buy.BarCode = sts.BarCode;
                        buy.ShopID = d.ShopId;
                        buy.Transfer = "N";
                        buy.balQty += (sts.WriteOnBoxQty * sts.BoxSize);
                        buy.WriteOn += (sts.WriteOnBoxQty * sts.BoxSize);
                        composite.AddRecordSet<Buy>(buy, OperationMode.Update, "", "Transfer,+balQty,+WriteOn", "BarCode,ShopID", "");
                    }
                }
                composite.AddRecordSet<TempInventory>(olist.First(), OperationMode.Delete, "TempId", "", "CREATE_BY", "");
                var response = _dal.InsertUpdateComposite(composite, ref errMsg);
                if (response == false || !string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                return chln;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveTempInventory(TempInventory model, out string errMsg)
        {
            errMsg = string.Empty;
            TempInventory temp = GetsTempInventory(null).Where(m => m.BarCode == model.BarCode && m.CREATE_BY != model.UserID).FirstOrDefault();
            if (temp != null)
            {
                if (temp.CREATE_BY == model.UserID)
                {
                    errMsg = "Another user already added this item";
                    return false; ;
                }
            }

            var SelectedItem = GetByBarcodeExpForInventory(model.BarCode);
            if (SelectedItem == null)
            {
                errMsg = "Item not found";
                return false;
            }

            if (SelectedItem.SaleBalQty > 0)
            {
                errMsg = "Sales Order is pending for this item, inventory not possible";
                return false;
            }
            if (SelectedItem == null)
            {
                errMsg = "Product Info Not found";
                return false;
            }
            decimal writeOff = model.WriteOffBoxQty;
            decimal writeOn = model.WriteOnBoxQty;
            decimal stockQty = model.stock;
            if (writeOn == 0 && writeOff == 0)
            {
                errMsg = "Enter write off or on qty";
                return false;
            }

            if (writeOn != 0 && writeOff != 0)
            {
                errMsg = "Invalid write off or on qty";
                return false;
            }
            if (stockQty <= 0)
            {
                if (writeOff > 0)
                {
                    errMsg = "Write off not possible for zero stock";
                    return false;
                }
            }

            TempInventory otemp = new();
            otemp.BarCode = model.BarCode;
            otemp.sBarCode = SelectedItem.sBarCode;
            otemp.StockBoxQty = model.stock;
            otemp.WriteOffBoxQty = model.WriteOffBoxQty;
            otemp.WriteOnBoxQty = model.WriteOnBoxQty;
            otemp.CREATE_BY = model.UserID;
            otemp.ProductDescription = model.ProductDescription;
            otemp.UnitUOM = model.UnitUOM;
            otemp.BoxSize = SelectedItem.BoxSize;
            otemp.RPU = SelectedItem.RPU;
            otemp.NewStockQty = model.NewStockQty;

            var response = _dal.Insert<TempInventory>(otemp, "", "TempId", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;

        }
    }
}
