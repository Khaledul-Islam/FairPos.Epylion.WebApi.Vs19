using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IMemberCategoryProductService
    {
        List<MemberCategoryProduct> FindByMemCatId(string MemCatId);
        MemberCategoryProduct FindById(string CatPrdID);
        List<MemberCategoryProduct> FindAll();
        bool Delete(MemberCategoryProduct model);
        bool Insert(MemberCategoryProduct model);
        bool UpdateEmployeeWiseProductLimit(string MemCatId);
    }
    public class MemberCategoryProductService : IMemberCategoryProductService
    {
        private readonly IMemberCategoryProductRepository _repo;

        public MemberCategoryProductService(IMemberCategoryProductRepository repo)
        {
            _repo = repo;
        }
        public bool Insert(MemberCategoryProduct model)
        {
            return _repo.Insert(model);
        }
        public bool UpdateEmployeeWiseProductLimit(string MemCatID)
        {
            return _repo.UpdateEmployeeWiseProductLimit(MemCatID);
        }
        public bool Delete(MemberCategoryProduct model)
        {
            return _repo.Delete(model);
        }
        public List<MemberCategoryProduct> FindAll()
        {
            return _repo.FindAll();
        }

        public MemberCategoryProduct FindById(string CatPrdID)
        {
            return _repo.FindById(CatPrdID);
        }

        public List<MemberCategoryProduct> FindByMemCatId(string MemCatId)
        {
            return _repo.FindByMemCatId(MemCatId);
        }
    }
}
