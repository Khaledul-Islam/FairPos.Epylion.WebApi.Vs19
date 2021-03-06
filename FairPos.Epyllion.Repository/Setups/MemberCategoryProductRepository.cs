using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    /////////////////////////////////////////////////////////////////////////////////
    //FileName: IMemberCategoryProductRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 29/09/2021
    //Last Modified On : 
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    /////////////////////////////////////////////////////////////////////////////////
    public interface IMemberCategoryProductRepository
    {
        List<MemberCategoryProduct> FindByMemCatId(string MemCatId);
        MemberCategoryProduct FindById(string CatPrdID);
        List<MemberCategoryProduct> FindAll();
        bool Delete(MemberCategoryProduct model);
        bool Insert(MemberCategoryProduct model);
        bool UpdateEmployeeWiseProductLimit(string MemCatId);
    }

    public class MemberCategoryProductRepository : BaseRepository, IMemberCategoryProductRepository
    {
        public MemberCategoryProductRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT  [CatPrdId]
                                 ,[MemCatId]
                                 ,[PrdID]
                                 ,[LimitQty]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY CatPrdId)  =1)
	                     THEN	(SELECT COUNT(*) FROM MemberCategoryProduct) 
	                     ELSE 0 END RecordCount
                    FROM MemberCategoryProduct";
        }

        public bool Insert(MemberCategoryProduct model)
        {
            model.CatPrdId = int.Parse(GetMaxId("EmpPrdID", "MemberCategoryProduct"));

            bool response = _dal.Insert<MemberCategoryProduct>(model, "", "CatPrdId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }

        public bool UpdateEmployeeWiseProductLimit(string MemCatId)
        {
            string q= @"SP_UpdateEmployeeProductLimit '" + MemCatId + "' ";
            List<string> query = new();
            query.Add(q);
            return _dal.ExecuteQuery(query, ref msg);
        }
        public bool Delete(MemberCategoryProduct model)
        {
            string Query = "SELECT * FROM MemberCategoryProduct WHERE MemCatId='" + model.MemCatId + "'";
            var data = _dal.Select<MemberCategoryProduct>(Query, ref msg).FirstOrDefault();

            if (data == null)
            {
                return false;
            }

            bool response = _dal.Delete<MemberCategoryProduct>(data, "MemCatId", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }
        public List<MemberCategoryProduct> FindAll()
        {
            query = baseQuery;
            var r = _dal.Select<MemberCategoryProduct>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public MemberCategoryProduct FindById(string CatPrdID)
        {
            query = baseQuery + $" where CatPrdId='{CatPrdID}' ";
            var r = _dal.SelectFirstOrDefault<MemberCategoryProduct>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public List<MemberCategoryProduct> FindByMemCatId(string MemCatId)
        {
            query = baseQuery + $" where MemCatId='{MemCatId}' ";
            var r = _dal.Select<MemberCategoryProduct>(query, ref msg).ToList();

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }
    }
}
