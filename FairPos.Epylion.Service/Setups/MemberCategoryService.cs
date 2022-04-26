using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IMemberCategoryService
    {
        bool IsDataExist(MemberCategory model);

        bool Insert(MemberCategory model);
        bool Update(MemberCategory model);
        bool Delete(MemberCategory model);
        List<MemberCategory> SelectAll();
        MemberCategory FindById(string userName);
        List<MemberCategory> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
    }

    public class MemberCategoryService : IMemberCategoryService
    {
        private readonly IMemberCategoryRepository _repo;
        public MemberCategoryService(IMemberCategoryRepository repo)
        {
            _repo = repo;
        }

        public bool Delete(MemberCategory model)
        {
            return _repo.Delete(model);
        }

        public MemberCategory FindById(string userName)
        {
            return _repo.FindById(userName);
        }

        public List<MemberCategory> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return _repo.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool Insert(MemberCategory model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return _repo.Insert(model);
        }

        public bool IsDataExist(MemberCategory model)
        {
            return _repo.IsDataExist(model);
        }

        public List<MemberCategory> SelectAll()
        {
            return _repo.SelectAll();
        }

        public bool Update(MemberCategory model)
        {

            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return _repo.Update(model);
        }
    }
}
