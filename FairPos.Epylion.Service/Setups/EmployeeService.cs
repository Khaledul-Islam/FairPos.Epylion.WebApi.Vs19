using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service
{

    public interface IEmployeeService
    {
        bool IsDataExist(Employee model);

        bool Insert(Employee model);
        bool Sync();
        bool Update(Employee model);
        bool Delete(Employee model);
        List<Employee> SelectAll();
        Employee FindById(string userName);
        List<Employee> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
    }

    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository repository;

        public EmployeeService(IEmployeeRepository _repo)
        {
            repository = _repo;
        }

        public bool IsDataExist(Employee model)
        {
            return repository.IsDataExist(model);
        }

        public bool Insert(Employee model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Insert(model);
        }

        public bool Update(Employee model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Update(model);
        }

        public bool Delete(Employee model)
        {
            return repository.Delete(model);
        }

        public List<Employee> SelectAll()
        {
            return repository.SelectAll();
        }


        public Employee FindById(string userName)
        {
            return repository.FindById(userName);
        }


        public List<Employee> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return repository.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool Sync()
        {
            return repository.Sync();
        }
    }
}
