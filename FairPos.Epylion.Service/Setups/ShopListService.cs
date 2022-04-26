using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IShopListService
    {
        bool IsDataExist(ShopList model);

        bool Insert(ShopList model);
        bool Update(ShopList model);
        bool Delete(ShopList model);
        List<ShopList> SelectAll();
        ShopList FindById(string userName);
        List<ShopList> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool SaveSoftwareSetting(GlobalSetup gs);
        GlobalSetup GetSoftwareSetting(string storeID, out string errMsg);
    }

    public class ShopListService : IShopListService
    {
        //private readonly ILogger<UserService> _logger;

        IShopListRepository repository;

        public ShopListService(IShopListRepository _repo)
        {
            repository = _repo;
        }

     
        public bool IsDataExist(ShopList model)
        {
            return repository.IsDataExist(model);
        }

        public bool Insert(ShopList model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Insert(model);
        }

        public bool Update(ShopList model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Update(model);
        }

        public bool Delete(ShopList model)
        {
            return repository.Delete(model);
        }

        public List<ShopList> SelectAll()
        {
            return repository.SelectAll();
        }


        public ShopList FindById(string userName)
        {
            return repository.FindById(userName);
        }


        public List<ShopList> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return repository.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool SaveSoftwareSetting(GlobalSetup gs)
        {
            return repository.SaveSoftwareSetting(gs);
        }

        public GlobalSetup GetSoftwareSetting(string storeID, out string errMsg)
        {
            return repository.GetSoftwareSetting(storeID,out errMsg);
        }
    }
    }
