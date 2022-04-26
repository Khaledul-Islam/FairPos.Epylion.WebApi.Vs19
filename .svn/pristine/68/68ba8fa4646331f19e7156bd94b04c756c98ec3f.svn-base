using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Sales
{
    public interface ISalesManRepository
    {
        SalesMan GetById(string salesManId);
        bool RemoveAllSalesMan(SalesMan model);
    }

    public class SalesManRepository : BaseRepository, ISalesManRepository
    {
        public SalesManRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
       
        }

        public SalesMan GetById(string salesManId)
        {
            string query = "Select * from SalesMan where SMID = '" + salesManId + "'";
            return _dal.Select<SalesMan>(query, ref msg).FirstOrDefault();
        }

        public bool RemoveAllSalesMan(SalesMan model)
        {
            return _dal.Delete<SalesMan>(model, "SMID", "", ref msg);
        }
    }
}
