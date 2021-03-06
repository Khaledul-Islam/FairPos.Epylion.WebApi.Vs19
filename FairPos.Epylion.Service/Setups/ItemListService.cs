using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IItemListService
    {
        bool Insert(StyleSize model);
        bool Update(StyleSize model);
        bool UpdateAuth(StyleSize model);
        bool Delete(StyleSize model);
        List<StyleSize> SelectAll();
        List<StyleSize> ItemDDL(string SupID, string PrdID);
        string GenerateSBarcode(string SupID, string PrdID);
        string GenerateBarcode();
        StyleSize EditItemDetailsBySbarcode(string sbarocde);
        StyleSize StyleSizeDetailsBySbarcode(string sbarocde);
        StyleSize StyleSizeDetailsByBarcode(string barocde);
        StyleSize ApprovedItembyBarcode(string barocde);
        List<StyleSize> ApproveItemListbySupID(string barocde);
        List<StyleSize> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(StyleSize model);
    }

    public class ItemListService : IItemListService
    {
        private readonly IItemListRepository _itemRepo;

        public ItemListService(IItemListRepository itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public bool Delete(StyleSize model)
        {
            return _itemRepo.Delete(model);
        }

        public StyleSize EditItemDetailsBySbarcode(string sbarocde)
        {
            return _itemRepo.EditItemDetailsBySbarcode(sbarocde);
        }

        public string GenerateSBarcode(string SupID, string PrdID)
        {
            return _itemRepo.GenerateSBarcode(SupID, PrdID);
        }
        public string GenerateBarcode()
        {
            return _itemRepo.GenerateBarcode();
        }

        public List<StyleSize> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return _itemRepo.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool Insert(StyleSize model)
        {
            return _itemRepo.Insert(model);
        }


        public bool IsDataExist(StyleSize model)
        {
            throw new NotImplementedException();
        }

        public List<StyleSize> ItemDDL(string SupID, string PrdID)
        {
            return _itemRepo.ItemDDL(SupID, PrdID);
        }

        public List<StyleSize> SelectAll()
        {
            return _itemRepo.SelectAll();
        }

        public StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            return _itemRepo.StyleSizeDetailsBySbarcode(sbarocde);
        }
        public StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            return _itemRepo.StyleSizeDetailsByBarcode(barocde);
        }
        public StyleSize ApprovedItembyBarcode(string barocde)
        {
            return _itemRepo.ApprovedItembyBarcode(barocde);
        }
        public List<StyleSize> ApproveItemListbySupID(string barocde)
        {
            return _itemRepo.ApproveItemListbySupID(barocde);
        }

        public bool Update(StyleSize model)
        {
            return _itemRepo.Update(model);
        }

        public bool UpdateAuth(StyleSize model)
        {
            return _itemRepo.UpdateAuth(model);
        }
    }
}
