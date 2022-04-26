using FairPos.Epylion.Models;
using FairPos.Epyllion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service
{

    public interface IProductService
    {
        bool IsDataExist(Product model);

        bool Insert(Product model);
        bool Update(Product model);
        bool Delete(Product model);
        List<Product> SelectAll();
        Product FindById(string userName);
        List<Product> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
    }

    public class ProductService : IProductService
    {

        IProductRepository repository;

        public ProductService(IProductRepository _repo)
        {
            repository = _repo;
        }


        public bool IsDataExist(Product model)
        {
            return repository.IsDataExist(model);
        }

        public bool Insert(Product model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Insert(model);
        }

        public bool Update(Product model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return repository.Update(model);
        }

        public bool Delete(Product model)
        {
            return repository.Delete(model);
        }

        public List<Product> SelectAll()
        {
            return repository.SelectAll();
        }


        public Product FindById(string userName)
        {
            return repository.FindById(userName);
        }


        public List<Product> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return repository.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }
    }
}
