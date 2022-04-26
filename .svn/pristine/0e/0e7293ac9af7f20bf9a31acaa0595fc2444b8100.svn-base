using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IEmployeeImageService
    {
        EmployeeImage FindById(string id);
    }

    public class EmployeeImageService : IEmployeeImageService
    {
        private readonly IEmployeeImageRepository _EMrepo;

        public EmployeeImageService(IEmployeeImageRepository eMrepo)
        {
            _EMrepo = eMrepo;
        }

        public EmployeeImage FindById(string id)
        {
            return _EMrepo.FindById(id);
        }
    }
}
