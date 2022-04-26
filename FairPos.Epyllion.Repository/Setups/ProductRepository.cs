using FairPos.Epylion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository
{

    public interface IProductRepository
    {
        bool Insert(Product model);
        bool Update(Product model);
        bool Delete(Product model);
        List<Product> SelectAll();
        Product FindById(string id);
        List<Product> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(Product model);
    }


    public class ProductRepository : BaseRepository , IProductRepository
    {


        public ProductRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT PrdID
                          ,PrdName
                          ,FloorID
                          ,VATPrcnt
                          ,DiscPrcnt
                          ,PrdNameBangla
                          ,CreateBy
                          ,CreateDate
                          ,UpdateBy
                          ,UpdateDate
                      
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY PrdID)  =1)
	                     THEN	(SELECT COUNT(*) FROM Product) 
	                     ELSE 0 END RecordCount
                    FROM Product";
        }

        public bool Insert(Product model)
        {
           

            model.PrdID = GetMaxId("PrdID", "00000", "00001", "Product");

            bool r = _dal.Insert<Product>(model, "", "", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public bool IsDataExist(Product model)
        {
            query = baseQuery + $" where (PrdID<>'{model.PrdID}' and '{model.PrdID}' <>'' ) and (PrdName='{model.PrdName}' or PrdNameBangla='{model.PrdNameBangla}' ) ";
            var r = _dal.SelectFirstOrDefault<Product>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (r == null)
                return false;
            return true;

        }


        public bool Update(Product model)
        {
            bool r = false;


            r = _dal.Update<Product>(model, "", "PrdID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Delete(Product model)
        {
            bool r = _dal.Delete<Product>(model, "PrdID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<Product> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<Product>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public Product FindById(string id)
        {
            query = baseQuery + $" where PrdID='{id}' ";
            var r = _dal.SelectFirstOrDefault<Product>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<Product> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where PrdID like '%" + searchText + @"%' or PrdName like '%" + searchText + @"%' or PrdNameBangla like '%" + searchText + @"%' 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<Product>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where PrdID like '%" + searchText + @"%' or PrdName like '%" + searchText + @"%' or PrdNameBangla like '%" + searchText + @"%' ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [Product] " + whereCluase;
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
