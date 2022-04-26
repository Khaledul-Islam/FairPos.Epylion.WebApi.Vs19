using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FairPos.Epyllion.Repository.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Sales
{
    public interface ISalesOrderService
    {
        List<SALES_ORDER> GetPendingPrintItems(string stockType);
        List<SALES_ORDER> GetOrderDetails(string so_no);
        BuyWorker GetProductInfoByBarcodeBuyWorker(string barcode, string supplierId);
        BuyStaff GetProductInfoByBarcodeBuyStaff(string barcode, string supplierId);
        Buy GetProductInfoByBarcodeBuyMain(string barcode, string supplierId);
        List<SALES_ORDER> GetPendingItemsByEmp(string empId, bool isAccApproved);
    }
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _repo;

        public SalesOrderService(ISalesOrderRepository repo)
        {
            _repo = repo;
        }
        public List<SALES_ORDER> GetOrderDetails(string so_no)
        {
            return _repo.GetOrderDetails(so_no);
        }
        public List<SALES_ORDER> GetPendingPrintItems(string stockType)
        {
            return _repo.GetPendingPrintItems(stockType);
        }
        public BuyWorker GetProductInfoByBarcodeBuyWorker(string barcode, string supplierId)
        {
            return _repo.GetProductInfoByBarcodeBuyWorker(barcode, supplierId);
        }
        public BuyStaff GetProductInfoByBarcodeBuyStaff(string barcode, string supplierId)
        {
            return _repo.GetProductInfoByBarcodeBuyStaff(barcode, supplierId);
        }
        public Buy GetProductInfoByBarcodeBuyMain(string barcode, string supplierId)
        {
            return _repo.GetProductInfoByBarcode(barcode, supplierId);
        }
        public List<SALES_ORDER> GetPendingItemsByEmp(string empId, bool isAccApproved)
        {
            return _repo.GetPendingItemsByEmp(empId, isAccApproved);
        }
    }
}
