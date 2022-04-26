using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Operations
{
    public interface IItemConversionService
    {
        bool DeleteTempConversionStock(string UserID);
        List<TempConversionStock> TempConvStockList();
        List<TempConversionStock> TempConvStockListByID(string UserID);
        List<Buy> GetProductsSupplier(string SupId, string ShopID);
        Buy GetByBarcodeExp(string barcode);
        List<StyleSizeCoversition> GetItemDetails(string ParentSbarcode, DateTime parentItemExpire);
        List<Buy> GetNewBarcodeBySbarcode(string sBarcode, decimal qty);
        bool SaveTempConvStock(TempConversionStock model);
        string SaveConvStock(List<StyleSizeCoversition> model, out string errMsg);
    }

    public class ItemConversionService : IItemConversionService
    {
        private readonly IItemConversionRepository _repo;

        public ItemConversionService(IItemConversionRepository repo)
        {
            _repo = repo;
        }

        public bool DeleteTempConversionStock(string UserID)
        {
           return _repo.DeleteTempConversionStock(UserID);
        }

        public Buy GetByBarcodeExp(string barcode)
        {
            return _repo.GetByBarcodeExp(barcode);
        }

        public List<StyleSizeCoversition> GetItemDetails(string ParentSbarcode, DateTime parentItemExpire)
        {
            return _repo.GetItemDetails(ParentSbarcode, parentItemExpire);
        }

        public List<Buy> GetNewBarcodeBySbarcode(string sBarcode, decimal qty)
        {
            return _repo.GetNewBarcodeBySbarcode(sBarcode, qty);
        }

        public List<Buy> GetProductsSupplier(string SupId, string ShopID)
        {
            return _repo.GetProductsSupplier(SupId,ShopID);
        }

        public string SaveConvStock(List<StyleSizeCoversition> model, out string errMsg)
        {
            return _repo.SaveConvStock(model,out errMsg);
        }

        public bool SaveTempConvStock(TempConversionStock model)
        {
            return _repo.SaveTempConvStock(model);
        }

        public List<TempConversionStock> TempConvStockList()
        {
            return _repo.TempConvStockList();
        }

        public List<TempConversionStock> TempConvStockListByID(string UserID)
        {
            return _repo.TempConvStockListByID(UserID);
        }
    }
}
