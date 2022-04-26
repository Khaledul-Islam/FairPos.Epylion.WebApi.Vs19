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
    public interface IInventoryService
    {
        Buy GetByBarcodeExpForInventory(string barcode);
        bool RemoveTempInventory(TempInventory model);
        List<TempInventory> GetsTempInventory(string name);
        bool SaveTempInventory(TempInventory model, out string errMsg);
        string SaveInventory(List<TempInventory> olist, out string errMsg);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;

        public InventoryService(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public Buy GetByBarcodeExpForInventory(string barcode)
        {
            return _repo.GetByBarcodeExpForInventory(barcode);
        }

        public List<TempInventory> GetsTempInventory(string name)
        {
            return _repo.GetsTempInventory(name);
        }

        public bool RemoveTempInventory(TempInventory model)
        {
            return _repo.RemoveTempInventory(model);
        }

        public string SaveInventory(List<TempInventory> olist, out string errMsg)
        {
            return _repo.SaveInventory(olist, out errMsg);
        }

        public bool SaveTempInventory(TempInventory model, out string errMsg)
        {
            return _repo.SaveTempInventory(model, out errMsg);
        }
    }
}
