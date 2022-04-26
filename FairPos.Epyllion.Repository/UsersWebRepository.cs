using FairPos.Epylion.Models;
using FIK.DAL;
using System;
using System.Collections.Generic;

namespace FairPos.Epyllion.Repository
{

    public interface IUsersWebRepository
    {
        bool Insert(UsersWeb model);
        bool Update(UsersWeb model);
        bool Delete(UsersWeb model);
        List<UsersWeb> SelectAll();
        UsersWeb FindById(string userId);
        List<UsersWeb> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
    }

    public class UsersWebRepository : BaseRepository , IUsersWebRepository
    {

        

        public UsersWebRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT UserId
                        ,Password
                        ,FullName
                        ,Address
                        ,Email
                        ,isActive
                        ,EmailUserId
                        ,EmailPassword
                        ,EmpId
                        ,UserType
                        ,Designation
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY UserId)  =1)
	                     THEN	(SELECT COUNT(*) FROM UsersWeb) 
	                     ELSE 0 END RecordCount
                    FROM UsersWeb";
        }

        public bool Insert(UsersWeb model)
        {

            CompositeModel compositeModel = new CompositeModel();
            compositeModel.AddRecordSet<UsersWeb>(model, OperationMode.Insert, "", "", "", "");
            compositeModel.AddRecordSet<UsersWeb>(model, OperationMode.Delete, "", "", "UserId", "UsersShop");
            compositeModel.AddRecordSet<UsersShop>(model.usersShop, OperationMode.Insert, "UsersShopId", "", "", "UsersShop");

            //bool r = _dal.Insert<UsersWeb>(model, "", "", "", ref msg);
            bool r = _dal.InsertUpdateComposite(compositeModel, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Update(UsersWeb model)
        {
            bool r = false;

            CompositeModel compositeModel = new CompositeModel();
           

            if (!string.IsNullOrEmpty(model.Password))
            {
               // r = _dal.Update<UsersWeb>(model, "Password,FullName,Address,Email,isActive,EmailUserId,EmailPassword,EmpId,Designation", "UserId", "", ref msg);
            compositeModel.AddRecordSet<UsersWeb>(model, OperationMode.Update, "", "Password,FullName,Address,Email,isActive,UserType,EmailUserId,EmailPassword,EmpId,Designation", "UserId", "");
            }
            else
            {
                //r = _dal.Update<UsersWeb>(model, "FullName,Address,Email,isActive,EmailUserId,EmailPassword,EmpId,Designation", "UserId", "", ref msg);
            compositeModel.AddRecordSet<UsersWeb>(model, OperationMode.Update, "", "FullName,Address,Email,isActive,UserType,EmailUserId,EmailPassword,EmpId,Designation", "UserId", "");
            }
            compositeModel.AddRecordSet<UsersWeb>(model, OperationMode.Delete, "", "", "UserId", "UsersShop");
            compositeModel.AddRecordSet<UsersShop>(model.usersShop, OperationMode.Insert, "UsersShopId", "", "", "UsersShop");

            r = _dal.InsertUpdateComposite(compositeModel, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Delete(UsersWeb model)
        {
            bool r = _dal.Delete<UsersWeb>(model,  "UserId", "", ref msg);
            if (r)
            {
                r = _dal.Delete<UsersWeb>(model, "UserId", "UsersShop", ref msg);
            }

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<UsersWeb> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<UsersWeb>(query,  ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public UsersWeb FindById(string userId)
        {
            query = baseQuery + $" where UserId='{userId}' ";
            var r = _dal.SelectFirstOrDefault<UsersWeb>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if(r != null)
            {
                query = $@"SELECT  UsersShop.UsersShopId,UsersShop.UserId,UsersShop.ShopID,ShopName
                        FROM UsersShop
                        INNER JOIN dbo.ShopList ON ShopList.ShopID = UsersShop.ShopID where UsersShop.UserId='{userId}' ";
                r.usersShop = _dal.Select<UsersShop>(query, ref msg);
            }

            return r;
        }


        public List<UsersWeb> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserId like '%" + searchText + @"%' or FullName like '%" + searchText + @"%' or Email like '%" + searchText + @"%'
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<UsersWeb>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserId like '%" + searchText + @"%' or FullName like '%" + searchText + @"%' or Email like '%" + searchText + @"%'";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [UsersWeb] " + whereCluase;
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
