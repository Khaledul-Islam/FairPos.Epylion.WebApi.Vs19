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
    public interface ISalesManService
    {
        SalesMan GetById(string salesManId);
        bool RemoveAllSalesMan(SalesMan model);
    }
    public class SalesManService : ISalesManService
    {
        private readonly ISalesManService _repo;

        public SalesManService(ISalesManService repo)
        {
            _repo = repo;
        }

        public SalesMan GetById(string salesManId)
        {
            return _repo.GetById(salesManId);
        }

        public bool RemoveAllSalesMan(SalesMan model)
        {
            return _repo.RemoveAllSalesMan(model);
        }
    }
}
