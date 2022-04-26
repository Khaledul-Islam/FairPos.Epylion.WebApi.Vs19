using FairPos.Epylion.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    /////////////////////////////////////////////////////////////////////////////////
    //FileName: IEmployeeItemRepository.cs
    //FileType: C# Source file
    //Author : RAHEE
    //Created On : 25/9/2021
    //Last Modified On : 
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    /////////////////////////////////////////////////////////////////////////////////
    public interface IEmployeeItemRepository
    {
        bool Insert(Employee model);
        bool Update(Employee model);
        bool Delete(Employee model);
        List<Employee> SelectAll();
        Employee FindById(string id);
        bool Sync ();
        List<Employee> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(Employee model);
    }

    public class EmployeeItemRepository : BaseRepository, IEmployeeItemRepository
    {


        public EmployeeItemRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [EmpID]
                          ,[RFCardNo]
                          ,[StaffType]
                          ,[vDesignation]
                          ,[vDesignationBangla]
                          ,[Department]
                          ,[DepartmentBangla]
                          ,[Name]
                          ,[NameBangla]
                          ,[Phone]
                          ,[Email]
                          ,[Unit]
                          ,[FPSEnrollment]
                          ,[FamilyMembers]
                          ,[DiscAllowed]
                          ,[IsTransfer]
                          ,[IsCreditAllowed]
                          ,[CreditLimit]
                          ,[IsActive]
                          ,[UpdateDate]
                          ,[SpouseId]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY EmpID)  =1)
	                     THEN	(SELECT COUNT(*) FROM Employee) 
	                     ELSE 0 END RecordCount
                    FROM Employee";
        }

        public bool Insert(Employee model)
        {

            model.EmpID = int.Parse(GetMaxId("EmpID", "Employee"));

            bool r = _dal.Insert<Employee>(model, "", "", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public bool IsDataExist(Employee model)
        {
            query = baseQuery + $" where (EmpID<>'{model.EmpID}' and '{model.EmpID}' <>'' ) and (Name='{model.Name}' ) ";
            var r = _dal.SelectFirstOrDefault<Employee>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (r == null)
                return false;
            return true;

        }


        public bool Update(Employee model)
        {
            bool r = false;


            r = _dal.Update<Employee>(model, "", "EmpID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Delete(Employee model)
        {
            bool r = _dal.Delete<Employee>(model, "EmpID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<Employee> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<Employee>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public Employee FindById(string id)
        {
            query = baseQuery + $" where EmpID='{id}' ";
            var r = _dal.SelectFirstOrDefault<Employee>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<Employee> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = "  where EmpID like '%" + searchText + @"%' or Name like '%" + searchText + @"%' or Email like '%" + searchText + @"%'  or StaffType like '%" + searchText + @"%' or Phone like '%" + searchText + @"%'  or vDesignation like '%" + searchText + @"%'  or RFCardNo like '%" + searchText + @"%'  
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<Employee>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where EmpID like '%" + searchText + @"%' or Name like '%" + searchText + @"%' or Email like '%" + searchText + @"%'  or StaffType like '%" + searchText + @"%' or Phone like '%" + searchText + @"%'  or vDesignation like '%" + searchText + @"%'  or RFCardNo like '%" + searchText + @"%'  ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [Employee] " + whereCluase;
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

        public bool Sync()
        {
            string q = "Sync_EMP_Data_Main";
            List<string> query = new List<string>();
            query.Add(q);            
            return _dal.ExecuteQuery(query, ref msg);      
           
        }
    }
}
