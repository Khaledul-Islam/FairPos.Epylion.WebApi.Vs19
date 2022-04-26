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
    public interface ICircularPriceChangedService
    {
        List<CircularPriceChangedDetail> GetCircularDetails(string cpno);
        List<CircularPriceChanged> GetPending();
        bool saveCircularPriceChangeApprove(List<CircularPriceChangedDetail> model, out string errMsg);
        bool saveTempPriceChanged(Buy model);
        bool savePriceChanged(List<TempPriceChanged> tempData, out string errMsg);
        bool deleteTempPriceChanged(TempPriceChanged model);
        List<TempPriceChanged> GetTempDataByUser(string userId);
    }

    public class CircularPriceChangedService : ICircularPriceChangedService
    {
        private readonly ICircularPriceChangedRepositroy _Repo;
        public CircularPriceChangedService(ICircularPriceChangedRepositroy Repo)
        {          
            _Repo = Repo;
        }

        public bool deleteTempPriceChanged(TempPriceChanged model)
        {
            return _Repo.deleteTempPriceChanged(model);
        }

        public List<CircularPriceChangedDetail> GetCircularDetails(string cpno)
        {
            return _Repo.GetCircularDetails(cpno);
        }

        public List<CircularPriceChanged> GetPending()
        {
            return _Repo.GetPending();
        }

        public List<TempPriceChanged> GetTempDataByUser(string userId)
        {
            return _Repo.GetTempDataByUser(userId);
        }

        public bool savePriceChanged(List<TempPriceChanged> tempData, out string errMsg)
        {
            return _Repo.savePriceChanged(tempData, out errMsg);
        }

        public bool saveTempPriceChanged(Buy model)
        {
            return _Repo.saveTempPriceChanged(model);
        }

        public bool saveCircularPriceChangeApprove(List<CircularPriceChangedDetail> model, out string errMsg)
        {
          
            return _Repo.saveCircularPriceChangeApprove(model, out errMsg);
        }

    }
}
