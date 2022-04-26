using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FairPos.Epyllion.Repository.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Transfer
{
    public interface IStaffToMainWorkerService
    {
        List<Buy> GetMainWorkerProducts(string SupId, string ShopID);
        List<Buy> GetMainWorkerNewBarcode(string sBarcode, decimal qty);
        Buy GetMainWorkerBarcodeExp(string expbarcode);
        List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null);
        string SaveStockTransfer(List<TempStockTransfer> model);
        bool SaveTempStockTransfer(Buy model);
    }

    public class StaffToMainWorkerService : IStaffToMainWorkerService
    {
        private readonly IStaffToMainWorkerRepository _service;

        public StaffToMainWorkerService(IStaffToMainWorkerRepository service)
        {
            _service = service;
        }

        public bool SaveTempStockTransfer(Buy model)
        {
            return _service.SaveTempStockTransfer(model);
        }
        public Buy GetMainWorkerBarcodeExp(string expbarcode)
        {
            return _service.GetMainWorkerBarcodeExp(expbarcode);
        }

        public List<Buy> GetMainWorkerNewBarcode(string sBarcode, decimal qty)
        {
            return _service.GetMainWorkerNewBarcode(sBarcode, qty);
        }
        public List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null)
        {
            return _service.GetsForDataTables(start, length, orderBy, ref msgs, UserID, ShopID, stocktype, searchText);
        }
        public List<Buy> GetMainWorkerProducts(string SupId, string ShopID)
        {
            return _service.GetMainWorkerProducts(SupId, ShopID);
        }

        public string SaveStockTransfer(List<TempStockTransfer> model)
        {
            return _service.SaveStockTransfer(model);
        }
    }
}
