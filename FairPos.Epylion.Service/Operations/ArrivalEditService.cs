using FairPos.Epylion.Models.Operations;
using FairPos.Epyllion.Repository.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Operations
{
    public interface IArrivalEditService
    {
        List<string> GetArrivalNoQC(string UserID, string ShopID);
        List<TempArrivalUpdate> GetTempArrivalUpdateByUser(string UserId);
        List<TempArrivalUpdate> GetsTempArrivalUpdate();
        List<TempArrivalUpdate> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null);
        public bool LoadTempArrivalUpdate(string arrivalNo, string UserID, out string errMsg);
        bool updateTmpArrival(TempArrivalUpdate item,out string errMsg);
        string updateArrival(TempArrivalUpdate item, out string errMsg);
        bool deleteTmpArrival(TempArrivalUpdate item);
    }

    public class ArrivalEditService : IArrivalEditService
    {
        private readonly IArrivalEditRepository _repo;

        public ArrivalEditService(IArrivalEditRepository repo)
        {
            _repo = repo;
        }

        public bool deleteTmpArrival(TempArrivalUpdate item)
        {
            return _repo.deleteTmpArrival(item);
        }

        public List<string> GetArrivalNoQC(string UserID, string ShopID)
        {
            return _repo.GetArrivalNoQC(UserID,ShopID);
        }

        public List<TempArrivalUpdate> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null)
        {
            return _repo.GetsForDataTables(start, length, orderBy, ref msgs, UserID, searchText);
        }

        public List<TempArrivalUpdate> GetsTempArrivalUpdate()
        {
            return _repo.GetsTempArrivalUpdate();
        }

        public List<TempArrivalUpdate> GetTempArrivalUpdateByUser(string UserId)
        {
            return _repo.GetTempArrivalUpdateByUser(UserId);
        }

        public bool LoadTempArrivalUpdate(string arrivalNo, string UserID, out string errMsg)
        {
            return _repo.LoadTempArrivalUpdate(arrivalNo, UserID, out errMsg);
        }

        public string updateArrival(TempArrivalUpdate item, out string errMsg)
        {
            return _repo.updateArrival(item, out errMsg);
        }

        public bool updateTmpArrival(TempArrivalUpdate item, out string errMsg)
        {
            return _repo.updateTmpArrival(item, out errMsg);
        }
    }
}
