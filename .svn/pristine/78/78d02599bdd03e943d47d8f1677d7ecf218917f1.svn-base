using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FileName: MeasureUnitRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 14/09/2021 9:56:39 AM
    //Last Modified On : 15/09/2021 04:00:00 PM
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IMeasureUnitRepository
    {
        bool Insert(MeasureUnit model);
        bool Update(MeasureUnit model);
        bool Delete(MeasureUnit model);
        List<MeasureUnit> SelectAll();
        MeasureUnit FindById(string id);
        List<MeasureUnit> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(MeasureUnit model);
    }
    public class MeasureUnitRepository : BaseRepository, IMeasureUnitRepository
    {


        public MeasureUnitRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT UOMId
                          ,UOMName
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY UOMId)  =1)
	                     THEN	(SELECT COUNT(*) FROM MeasureUnit) 
	                     ELSE 0 END RecordCount
                    FROM MeasureUnit";
        }

        public bool Insert(MeasureUnit model)
        {

            model.UOMId = int.Parse( GetMaxId("UOMId", "MeasureUnit"));

            bool response = _dal.Insert<MeasureUnit>(model, "", "", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }

        public bool IsDataExist(MeasureUnit model)
        {
            query = baseQuery + $" where (UOMId<>'{model.UOMId}' and '{model.UOMId}' <>'' ) and (UOMName='{model.UOMName}' ) ";
            var response = _dal.SelectFirstOrDefault<MeasureUnit>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (response == null)
                return false;
            return true;

        }


        public bool Update(MeasureUnit model)
        {
            bool response = false;


            response = _dal.Update<MeasureUnit>(model, "", "UOMId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public bool Delete(MeasureUnit model)
        {
            bool response = _dal.Delete<MeasureUnit>(model, "UOMId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<MeasureUnit> SelectAll()
        {
            query = baseQuery;
            var response = _dal.Select<MeasureUnit>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public MeasureUnit FindById(string id)
        {
            query = baseQuery + $" where UOMId='{id}' ";
            var response = _dal.SelectFirstOrDefault<MeasureUnit>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<MeasureUnit> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UOMId like '%" + searchText + @"%' or UOMName like '%" + searchText + @"%'  
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<MeasureUnit>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UOMId like '%" + searchText + @"%' or UOMName like '%" + searchText + @"%' ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [MeasureUnit] " + whereCluase;
            var count = _dal.SelectFirstColumn(query, ref msg);
            if (data != null && data.Count > 0)
            {
                int _count;
                int.TryParse(count, out _count);
                data[0].RecordFilter = _count;
            }
            #endregion

            msgs = msg;
            return data;
        }


    }
}
