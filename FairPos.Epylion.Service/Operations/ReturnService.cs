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
    public interface IReturnService
    {
        List<ReasonsForReturn> GetReturnReasonDDL();
        List<TempStockReturnShop> GetTempDataByUser(string userId, string shopId);
        TempStockReturnShop GetTempDataByBarcode(string barcode);
        bool TempReturnSave(Buy model);
        bool TempReturnDelete(TempStockReturnShop model);
        string StockReturnShopSave(List<TempStockReturnShop> model);
        List<TempStockReturnShop> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null);
    }

    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _repo;

        public ReturnService(IReturnRepository repo)
        {
            _repo = repo;
        }

        public List<ReasonsForReturn> GetReturnReasonDDL()
        {
            return _repo.GetReturnReasonDDL();
        }

        public List<TempStockReturnShop> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null)
        {
            return _repo.GetsForDataTables(start, length, orderBy, ref msgs, UserID, ShopID, searchText);
        }

        public TempStockReturnShop GetTempDataByBarcode(string barcode)
        {
            return _repo.GetTempDataByBarcode(barcode);
        }

        public List<TempStockReturnShop> GetTempDataByUser(string userId, string shopId)
        {
            return _repo.GetTempDataByUser(userId, shopId);
        }

        public string StockReturnShopSave(List<TempStockReturnShop> model)
        {
            return _repo.StockReturnShopSave(model);
        }

        public bool TempReturnDelete(TempStockReturnShop model)
        {
            return _repo.TempReturnDelete(model);
        }

        public bool TempReturnSave(Buy model)
        {
            return _repo.TempReturnSave(model);
        }
    }
}
