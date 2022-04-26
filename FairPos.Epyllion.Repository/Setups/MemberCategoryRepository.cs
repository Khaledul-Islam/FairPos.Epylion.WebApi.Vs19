using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FileName: MemberCategoryRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 03/10/2021 9:56:39 AM
    //Last Modified On :
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IMemberCategoryRepository
    {
        bool Insert(MemberCategory model);
        bool Update(MemberCategory model);
        bool Delete(MemberCategory model);
        List<MemberCategory> SelectAll();
        MemberCategory FindById(string id);
        List<MemberCategory> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(MemberCategory model);
    }

    public class MemberCategoryRepository : BaseRepository, IMemberCategoryRepository
    {
        public MemberCategoryRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [MemCatId]
                                ,[MemCatName]
                                ,[MinValue]
                                ,[MaxValue]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY MemCatId)  =1)
	                     THEN	(SELECT COUNT(*) FROM MemberCategory) 
	                     ELSE 0 END RecordCount
                    FROM MemberCategory";
        }
        public bool Insert(MemberCategory model)
        {

            model.MemCatId = int.Parse(GetMaxId("MemCatId", "MemberCategory"));

            bool response = _dal.Insert<MemberCategory>(model, "", "", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }

        public bool IsDataExist(MemberCategory model)
        {
            query = baseQuery + $" where (MemCatId<>'{model.MemCatId}' and '{model.MemCatId}' <>'' ) and (MemCatName='{model.MemCatName}' ) ";
            var response = _dal.SelectFirstOrDefault<MemberCategory>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (response == null)
                return false;
            return true;

        }


        public bool Update(MemberCategory model)
        {
            bool response = _dal.Update<MemberCategory>(model, "", "MemCatId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public bool Delete(MemberCategory model)
        {
            bool response = _dal.Delete<MemberCategory>(model, "MemCatId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<MemberCategory> SelectAll()
        {
            query = baseQuery;
            var response = _dal.Select<MemberCategory>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public MemberCategory FindById(string id)
        {
            query = baseQuery + $" where MemCatId='{id}' ";
            var response = _dal.SelectFirstOrDefault<MemberCategory>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<MemberCategory> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where MemCatId like '%" + searchText + @"%' or MemCatName like '%" + searchText + @"%'  
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<MemberCategory>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where MemCatId like '%" + searchText + @"%' or MemCatName like '%" + searchText + @"%' ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [MemberCategory] " + whereCluase;
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
