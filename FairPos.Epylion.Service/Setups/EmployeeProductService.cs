using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IEmployeeProductService
    {
        bool Insert(EmployeeProduct model);
        bool Update(EmployeeProduct model);
        bool Delete(EmployeeProduct model);
        List<EmployeeProduct> SelectAll();
        EmployeeProduct FindById(string id);
        List<EmployeeProduct> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(EmployeeProduct model);
        Creditlimit GetCreditLimit(string cusId);
        List<EmployeeProduct> GetEmployeeProductItem(string cusID);
    }

    public class EmployeeProductService : IEmployeeProductService
    {
        private readonly IEmployeeProductRepository _ePRepo;

        public EmployeeProductService(IEmployeeProductRepository ePRepo)
        {
            _ePRepo = ePRepo;
        }

        public bool Delete(EmployeeProduct model)
        {
            return _ePRepo.Delete(model);
        }

        public EmployeeProduct FindById(string id)
        {
            return _ePRepo.FindById(id);
        }

        public List<EmployeeProduct> GetEmployeeProductItem(string cusID)
        {
            return _ePRepo.GetEmployeeProductItem(cusID);
        }
        public Creditlimit GetCreditLimit(string cusId)
        {
            return _ePRepo.GetCreditLimit(cusId);
        }

        public List<EmployeeProduct> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return _ePRepo.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool Insert(EmployeeProduct model)
        {
            return _ePRepo.Insert(model);
        }

        public bool IsDataExist(EmployeeProduct model)
        {
            return _ePRepo.IsDataExist(model);
        }

        public List<EmployeeProduct> SelectAll()
        {
            return _ePRepo.SelectAll();
        }

        public bool Update(EmployeeProduct model)
        {
            return _ePRepo.Update(model);
        }
    }
}
