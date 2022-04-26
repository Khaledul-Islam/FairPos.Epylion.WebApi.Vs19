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
    public interface IWorkerToMainStaffService
    {
        List<Buy> GetWorkerStaffProducts(string SupId, string ShopID);
        List<Buy> GetWorkerStaffNewBarcode(string sBarcode, decimal qty);
        Buy GetWorkerStaffBarcodeExp(string expbarcode);
        string SaveStockTransfer(List<TempStockTransfer> model);
        bool SaveTempStockTransfer(Buy model);
        List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null);
    }
    public class WorkerToMainStaffService : IWorkerToMainStaffService
    {
        private readonly IWorkerToMainStaffRepository _repo;

        public WorkerToMainStaffService(IWorkerToMainStaffRepository repo)
        {
            _repo = repo;
        }

        public Buy GetWorkerStaffBarcodeExp(string expbarcode)
        {
            return _repo.GetWorkerStaffBarcodeExp(expbarcode);
        }
        public bool SaveTempStockTransfer(Buy model)
        {
            return _repo.SaveTempStockTransfer(model);
        }
        public List<Buy> GetWorkerStaffNewBarcode(string sBarcode, decimal qty)
        {
            return _repo.GetWorkerStaffNewBarcode(sBarcode, qty);
        }
        public List<TempStockTransfer> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string stocktype, string searchText = null)
        {
            return _repo.GetsForDataTables(start, length, orderBy, ref msgs, UserID, ShopID, stocktype, searchText);
        }
        public List<Buy> GetWorkerStaffProducts(string SupId, string ShopID)
        {
            return _repo.GetWorkerStaffProducts(SupId, ShopID);
        }

        public string SaveStockTransfer(List<TempStockTransfer> model)
        {
            return _repo.SaveStockTransfer(model);
        }
    }
}
