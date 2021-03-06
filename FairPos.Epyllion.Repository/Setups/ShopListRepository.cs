using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository
{

    public interface IShopListRepository
    {
        bool Insert(ShopList model);
        bool Update(ShopList model);
        bool Delete(ShopList model);
        List<ShopList> SelectAll();
        ShopList FindById(string id);
        List<ShopList> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(ShopList model);
        bool SaveSoftwareSetting(GlobalSetup gs);
        GlobalSetup GetSoftwareSetting(string storeID, out string errMsg);
    }

    public class ShopListRepository : BaseRepository, IShopListRepository
    {


        public ShopListRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT ShopID
                        ,ShopName
                        ,VillAreaRoad
                        ,Post
                        ,Pstation
                        ,District
                        ,Contact
                        ,Phone
                        ,VatRegNo
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY ShopID)  =1)
	                     THEN	(SELECT COUNT(*) FROM ShopList) 
	                     ELSE 0 END RecordCount
                    FROM ShopList";
        }

        public bool Insert(ShopList model)
        {

            model.ShopID = GetMaxIdWithPrfix("ShopID", "2", "01", "ShopList", "RF");

            bool r = _dal.Insert<ShopList>(model, "", "", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public bool IsDataExist(ShopList model)
        {
            query = baseQuery + $" where (ShopID<>'{model.ShopID}' and '{model.ShopID}' <>'' ) and ShopName='{model.ShopName}' ";
            var r = _dal.SelectFirstOrDefault<ShopList>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (r == null)
                return false;
            return true;

        }


        public bool Update(ShopList model)
        {
            bool r = false;


            r = _dal.Update<ShopList>(model, "", "ShopID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Delete(ShopList model)
        {
            bool r = _dal.Delete<ShopList>(model, "ShopID", "", ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<ShopList> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<ShopList>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public ShopList FindById(string id)
        {
            query = baseQuery + $" where ShopID='{id}' ";
            var r = _dal.SelectFirstOrDefault<ShopList>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<ShopList> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where ShopID like '%" + searchText + @"%' or ShopName like '%" + searchText + @"%' or Contact like '%" + searchText + @"%' or Phone like '%" + searchText + @"%' or VatRegNo like '%" + searchText + @"%' or VillAreaRoad like '%" + searchText + @"%'
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<ShopList>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where ShopID like '%" + searchText + @"%' or ShopName like '%" + searchText + @"%' or Contact like '%" + searchText + @"%' or Phone like '%" + searchText + @"%' or VatRegNo like '%" + searchText + @"%' or VillAreaRoad like '%" + searchText + @"%'";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [ShopList] " + whereCluase;
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

        public bool SaveSoftwareSetting(GlobalSetup global)
        {
            GlobalSetup gs = new();
            if (gs == null)
                return false;

            gs.IsVatAfterDiscount = global.IsVatAfterDiscount;
            gs.IsLargeInvoice = global.IsLargeInvoice;
            gs.AttandanceRequired = global.AttandanceRequired;
            gs.discount = global.discount;
            gs.DecimalLengeth = global.DecimalLengeth;
            gs.StoreId = global.StoreId;

            gs.BankCode = global.BankCode;
            gs.ExpenseCode = global.ExpenseCode;

            gs.IS_BLOCK_WORKER = global.IS_BLOCK_WORKER;
            gs.BW_FROMDATE = global.BW_FROMDATE;
            gs.BW_TODATE = global.BW_TODATE;

            gs.IS_BLOCK_NONMGMSTAFF = global.IS_BLOCK_NONMGMSTAFF;
            gs.NMS_FROMDATE = global.NMS_FROMDATE;
            gs.NMS_TODATE = global.NMS_TODATE;

            gs.PO_Email = global.PO_Email;
            gs.SalesInvoice_Email = global.SalesInvoice_Email;
            gs.smtpAddress = global.smtpAddress;
            gs.smtpPort = global.smtpPort;

            gs.Topup_email = global.Topup_email;
            gs.MinTopup = global.MinTopup;
            gs.MaxTopup = global.MaxTopup;

            gs.EnableSsl = global.EnableSsl;
            gs.SenderEmail = global.SenderPass;
            gs.SenderPass= global.SenderPass;

            gs.IS_REQ_ENABLE = global.IS_REQ_ENABLE;
            gs.REQ_TODATE = global.REQ_TODATE;
            gs.REQ_FROMDATE = global.REQ_FROMDATE;
            
            gs.ARR_FROMDATE = global.ARR_FROMDATE;
            gs.ARR_TODATE = global.ARR_TODATE;
            gs.IS_ARR_ENABLE = global.IS_ARR_ENABLE;

            CompositeModel composite = new();
            string cols = "AttandanceRequired,BW_FROMDATE,BW_TODATE,BankCode,DecimalLengeth,ExpenseCode,IS_BLOCK_NONMGMSTAFF," +
                "IS_BLOCK_WORKER,IsLargeInvoice,IsVatAfterDiscount,MaxTopup,MinTopup,NMS_FROMDATE,NMS_TODATE,PO_Email,SalesInvoice_Email,StoreId," +
                "Topup_email,discount,smtpAddress,smtpPort,GlobalSetupID,SenderEmail,SenderPass,EnableSsl,IS_REQ_ENABLE,REQ_TODATE,REQ_FROMDATE,ARR_TODATE,ARR_FROMDATE,IS_ARR_ENABLE";
            composite.AddRecordSet<GlobalSetup>(gs, OperationMode.InsertOrUpdaet, "GlobalSetupID", cols, "StoreId", "");
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                return false;
            }
            return response;
        }

        public GlobalSetup GetSoftwareSetting(string storeID,out string errMsg)
        {
            errMsg = string.Empty;
            string sql = "select * from GlobalSetup where StoreId='"+storeID+"'";
            var response = _dal.Select<GlobalSetup>(sql, ref msg).FirstOrDefault();
            if (response == null)
            {
                errMsg = "Shop not configured yet";
                return new GlobalSetup();
            }
            return response;
        }
    }
}
