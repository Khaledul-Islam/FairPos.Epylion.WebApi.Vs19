using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository;
using FairPos.Epyllion.Repository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Setups
{
    public interface IMeasureUnitService
    {
        bool IsDataExist(MeasureUnit model);

        bool Insert(MeasureUnit model);
        bool Update(MeasureUnit model);
        bool Delete(MeasureUnit model);
        List<MeasureUnit> SelectAll();
        MeasureUnit FindById(string userName);
        List<MeasureUnit> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
    }

    public class MeasureUnitService : IMeasureUnitService
    {
        private readonly IMeasureUnitRepository _mRepo;

        public MeasureUnitService(IMeasureUnitRepository mRepo)
        {
            _mRepo = mRepo;
        }

        public bool Delete(MeasureUnit model)
        {
            return _mRepo.Delete(model);
        }

        public MeasureUnit FindById(string userName)
        {
            return _mRepo.FindById(userName);
        }

        public List<MeasureUnit> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            return _mRepo.GetsForDataTables(start, length, orderBy, ref msgs, searchText);
        }

        public bool Insert(MeasureUnit model)
        {
            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return _mRepo.Insert(model);
        }

        public bool IsDataExist(MeasureUnit model)
        {
            return _mRepo.IsDataExist(model);
        }

        public List<MeasureUnit> SelectAll()
        {
            return _mRepo.SelectAll();
        }

        public bool Update(MeasureUnit model)
        {

            if (IsDataExist(model))
            {
                throw new Exception("Duplicate record");
            }
            return _mRepo.Update(model);
        }
    }
}
