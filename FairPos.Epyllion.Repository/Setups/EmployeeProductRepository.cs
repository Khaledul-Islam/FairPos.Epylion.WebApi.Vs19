using Dapper;
using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    /////////////////////////////////////////////////////////////////////////////////
    //FileName: EmployeeProductRepository.cs
    //FileType: C# Source file
    //Author : SHUVO
    //Created On : 26/09/2021
    //Last Modified On : 
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    /////////////////////////////////////////////////////////////////////////////////
    public interface IEmployeeProductRepository
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

    public class EmployeeProductRepository : BaseRepository, IEmployeeProductRepository
    {
        public EmployeeProductRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [EmpPrdID]
                                ,[EmpID]
                                ,[PrdID]
                                ,[LimitQty]
                                ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY EmpPrdID)  =1)
	                             THEN	(SELECT COUNT(*) FROM EmployeeProduct) 
	                             ELSE 0 END RecordCount
                            FROM EmployeeProduct";
        }

        public bool Insert(EmployeeProduct model)
        {
            //string Query = "INSERT INTO EmployeeProduct(EmpID, PrdID, LimitQty)  VALUES('"+model.EmpID+ "', '" + model.PrdID + "', '" + model.LimitQty + "')";
            //var response = _dapper.Query<EmployeeProduct>(Query);
            //if(response!=null)
            //{
            //    return true;
            //}
            model.EmpPrdID = int.Parse(GetMaxId("EmpPrdID", "EmployeeProduct"));

            bool response = _dal.Insert<EmployeeProduct>(model, "", "EmpPrdID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }

        public bool IsDataExist(EmployeeProduct model)
        {
            query = baseQuery + $" where (EmpPrdID<>'{model.EmpPrdID}' and '{model.EmpPrdID}' <>'' ) and (EmpID='{model.EmpID}' ) ";
            var response = _dal.SelectFirstOrDefault<EmployeeProduct>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (response == null)
                return false;
            return true;

        }


        public bool Update(EmployeeProduct model)
        {
            bool response = false;


            response = _dal.Update<EmployeeProduct>(model, "", "EmpPrdID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public bool Delete(EmployeeProduct model)
        {
            string Query = "SELECT * FROM EmployeeProduct WHERE EmpID='" + model.EmpID + "'";
            var data = _dal.Select<EmployeeProduct>(Query, ref msg).FirstOrDefault();

            if(data==null)
            {
                return false;
            }

            bool response = _dal.Delete<EmployeeProduct>(data, "EmpID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<EmployeeProduct> SelectAll()
        {
            query = baseQuery;
            var response = _dal.Select<EmployeeProduct>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public EmployeeProduct FindById(string id)
        {
            query = baseQuery + $" where EmpPrdID='{id}' ";
            var response = _dal.SelectFirstOrDefault<EmployeeProduct>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }


        public List<EmployeeProduct> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where EmpPrdID like '%" + searchText + @"%' or EmpID like '%" + searchText + @"%'  
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<EmployeeProduct>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where EmpPrdID like '%" + searchText + @"%' or EmpID like '%" + searchText + @"%' ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [EmployeeProduct] " + whereCluase;
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

        public Creditlimit GetCreditLimit(string cusId)
        {
            string query = @"SP_Get_CreditLmit '" + cusId + "' ";
            var obj = _dal.Select<Creditlimit>(query, ref msg).FirstOrDefault();
            return obj;
        }
        public List<EmployeeProduct> GetEmployeeProductItem(string cusID)
        {
            string Query = "SELECT * FROM EmployeeProduct where EmpID='" + cusID + "'";
            var items = _dal.Select<EmployeeProduct>(Query, ref msg).ToList();

            return items;
        }
    }
}
