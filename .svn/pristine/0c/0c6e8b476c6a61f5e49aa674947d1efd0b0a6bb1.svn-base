using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service.Setups;
using FairPos.Epyllion.Repository.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Operations
{
    public interface IPurchaseOrderService
    {
        bool Insert(BuyOrderTemp model);
        List<Supplier> SupplierListDDL();
        string PurchaseOrder(List<BuyOrderTemp> model);
        Supplier SupplierbyID(string SupID);
        bool RemoveBuyOrderTemp(BuyOrderTemp model);
        bool RemoveBuyOrderTempByID(string barCode);
        bool RemoveBuyOrderTempEditByID(string barCode);
        List<BuyOrderTemp> GetBuyOrderTemps(string UserID);
        decimal GetActualStockBySbarocde(string sBarcode);
        List<BuyOrderTemp> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null);
        List<BuyOrderTempEdit> GetsForDataTablesPOEdit(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null);
        //PO Edit
        List<string> GetPONotReceived(string supplierId);
        bool POSaveTemp(string pono, string userid, string shopid, out string errMsg);
        bool DeletePOEditAll(string userid, string shopid, string chln, out string errMsg);
        string PurchaseOrderEditSave(List<BuyOrderTemp> model, out string errMsg);
        bool updateTmpPurchaseEdit(BuyOrderTempEdit item, out string errMsg);
        bool InsertPOEdit(BuyOrderTemp model);
    }

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ISupplierService _supService;
        private readonly IPurchaseOrderRepository _poRepo;
        public PurchaseOrderService(ISupplierService supService, IPurchaseOrderRepository poRepo)
        {
            _supService = supService;
            _poRepo = poRepo;
        }

        public List<Supplier> SupplierListDDL()
        {
            return _supService.SelectAll().ToList();
        }
        public bool Insert(BuyOrderTemp model)
        {
            return _poRepo.Insert(model);
        }

        public bool RemoveBuyOrderTemp(BuyOrderTemp model)
        {
            return _poRepo.RemoveBuyOrderTemp(model);
        }

        public Supplier SupplierbyID(string SupID)
        {
            return _supService.FindById(SupID);
        }

        public List<BuyOrderTemp> GetBuyOrderTemps(string UserID)
        {
            return _poRepo.GetBuyOrderTemps(UserID);
        }

        public bool RemoveBuyOrderTempByID(string barCode)
        {
            return _poRepo.RemoveBuyOrderTempByID(barCode);
        }
        public bool RemoveBuyOrderTempEditByID(string barCode)
        {
            return _poRepo.RemoveBuyOrderTempEditByID(barCode);
        }

        public decimal GetActualStockBySbarocde(string sBarcode)
        {
            return _poRepo.GetActualStockBySbarocde(sBarcode);
        }

        public string PurchaseOrder(List<BuyOrderTemp> model)
        {
            return _poRepo.PurchaseOrder(model);
        }

        public List<BuyOrderTemp> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null)
        {
            return _poRepo.GetsForDataTables(start, length, orderBy, ref msgs, UserID, searchText);
        }
        public List<BuyOrderTempEdit> GetsForDataTablesPOEdit(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null)
        {
            return _poRepo.GetsForDataTablesPOEdit(start, length, orderBy, ref msgs, UserID, ShopID, searchText);
        }

        public List<string> GetPONotReceived(string supplierId)
        {
            return _poRepo.GetPONotReceived(supplierId);
        }
        public bool POSaveTemp(string pono, string userid, string shopid, out string errMsg)
        {
            return _poRepo.POSaveTemp(pono, userid, shopid, out errMsg);
        }

        public bool DeletePOEditAll(string userid, string shopid, string chln, out string errMsg)
        {
            return _poRepo.DeletePOEditAll(userid, shopid, chln, out errMsg);
        }

        public string PurchaseOrderEditSave(List<BuyOrderTemp> model, out string errMsg)
        {
            return _poRepo.PurchaseOrderEditSave(model, out errMsg);
        }
        public bool updateTmpPurchaseEdit(BuyOrderTempEdit item, out string errMsg)
        {
            return _poRepo.updateTmpPurchaseEdit(item, out errMsg);
        }

        public bool InsertPOEdit(BuyOrderTemp model)
        {
            return _poRepo.InsertPOEdit(model);
        }
    }
}
