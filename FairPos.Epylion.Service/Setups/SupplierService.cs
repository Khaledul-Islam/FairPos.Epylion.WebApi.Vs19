using FairPos.Epylion.Models;
using FairPos.Epyllion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{

    public interface ISupplierService
    {
        bool IsDataExist(Supplier model);

        bool Insert(Supplier model);
        bool Update(Supplier model);
        bool Delete(Supplier model);
        List<Supplier> SelectAll();
        Supplier FindById(string userName);
        List<Supplier> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        SupplierDoc FindDocumentById(string id);
    }

    public class SupplierService : ISupplierService
    {

        ISupplierRepository repository;

        public SupplierService(ISupplierRepository _repo)
        {
            repository = _repo;
        }


        public bool IsDataExist(Supplier model)
        {
            return repository.IsDataExist(model);
        }

        public bool Insert(Supplier model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Insert(model);
        }

        public bool Update(Supplier model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Update(model);
        }

        public bool Delete(Supplier model)
        {
            return repository.Delete(model);
        }

        public List<Supplier> SelectAll()
        {
            return repository.SelectAll();
        }


        public Supplier FindById(string userName)
        {
            return repository.FindById(userName);
        }

        public SupplierDoc FindDocumentById(string id)
        {
            return repository.FindDocumentById(id);
        }


        public List<Supplier> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return repository.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

    }
}
