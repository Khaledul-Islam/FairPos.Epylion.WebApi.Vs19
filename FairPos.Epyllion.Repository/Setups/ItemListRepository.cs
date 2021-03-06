using Dapper;
using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Setups
{
    /////////////////////////////////////////////////////////////////////////////////
    //FileName: ItemListRepository.cs
    //FileType: C# Source file
    //Author : SHUVO,RAHEE
    //Created On : 15/09/2021
    //Last Modified On : 
    //Copy Rights : MediaSoft Data System LTD
    //Description : Interface and Class for defining database related functions
    /////////////////////////////////////////////////////////////////////////////////
    public interface IItemListRepository
    {
        bool Insert(StyleSize model);
        bool Update(StyleSize model);
        bool UpdateAuth(StyleSize model);
        bool Delete(StyleSize model);
        List<StyleSize> SelectAll();
        StyleSize EditItemDetailsBySbarcode(string sbarocde);
        List<StyleSize> ItemDDL(string SupID, string PrdID);
        List<StyleSize> ApproveItemListbySupID(string SupID);
        string GenerateSBarcode(string SupID, string PrdID);
        string GenerateBarcode();
        StyleSize StyleSizeDetailsBySbarcode(string sbarocde);
        StyleSize StyleSizeDetailsByBarcode(string barocde);
        StyleSize ApprovedItembyBarcode(string barocde);
        List<StyleSize> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(StyleSize model);
    }
    public class ItemListRepository : BaseRepository, IItemListRepository
    {
        public ItemListRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {

            baseQuery = @"SELECT [CMPIDX]
                              ,[sBarcode]
                              ,[Barcode]
                              ,[PrdID]
                              ,[ItemName]
                              ,[ItemNameBangla]
                              ,[DiscPrcnt]
                              ,[VATPrcnt]
                              ,[PrdComm]
                              ,[CPU]
                              ,[RPU]
                              ,[RPP]
                              ,[WSP]
                              ,[WSQ]
                              ,[DisContinued]
                              ,[SupID]
                              ,[UserID]
                              ,[ENTRYDT]
                              ,[ZoneID]
                              ,[Point]
                              ,[Reorder]
                              ,[MaxOrder]
                              ,[UOMId]
                              ,[ExpireLimit]
                              ,[AutoSale]
                              ,[BoxSize]
                              ,[BoxUOMId]
                              ,[IsConverationItem]
                              ,[MinOrder]
                              ,[ArrivalExpireLimit]
                              ,[POPackSize]
                              ,[PackUOMId]
                              ,[ItemWeight]
                              ,[IsEssentialItem]
                              ,[PrdName]
                              ,[isTrail]
                              ,[trailFrom]
                              ,[trailTo]
                              ,[ShopID]
                              ,[UOMName]
                              ,[PackUiomName]
                              ,[BOXUOMName]
                              ,[Supname]
                              ,[balQty]
                              ,[ItemFullName]
                              ,[isAuthorized],
                    CASE WHEN (ROW_NUMBER() OVER (ORDER BY CMPIDX)  =1)
	                                         THEN	(SELECT COUNT(*) FROM vStyleSize) 
	                                         ELSE 0 END RecordCount
                                      FROM [vStyleSize]";
        }
        public bool Delete(StyleSize model)
        {
            string BuyQuery = "select * from Buy where sBarcode='" + model.sBarcode + "'";
            var BuyResponse = _dal.Select<Buy>(BuyQuery, ref msg);
            if (!BuyResponse.Count().Equals(0) || BuyResponse.Count() > 0)
            {
                msg = "This record is in used";
                return false;
            }
            else
            {
                string ConvQuery = "delete from StyleSizeCoversition where MainSBarcode='" + model.sBarcode + "'" +
                    "SELECT @@ROWCOUNT";
                string StyleQuery = "delete from StyleSize where sBarcode='" + model.sBarcode + "'" +
                    "SELECT @@ROWCOUNT";
                if (_dapper.State == ConnectionState.Closed)
                    _dapper.Open();
                using (var transaction = _dapper.BeginTransaction())
                {
                    int rowaffect = _dapper.Query<int>(ConvQuery, transaction: transaction).FirstOrDefault();
                    int rowaffec = _dapper.Query<int>(StyleQuery, transaction: transaction).FirstOrDefault();
                    transaction.Commit();
                }
                return true;
            }

        }
        public List<StyleSize> ApproveItemListbySupID(string SupID)
        {
            var query = baseQuery + " where SupID='" + SupID + "' AND isAuthorized=1";
            var data = _dal.Select<StyleSize>(query, ref msg).ToList();
            return data;
        }
        public StyleSize EditItemDetailsBySbarcode(string sbarocde)
        {
            var query = baseQuery + " where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();

            //grid load
            if (data.IsConverationItem)
            {
                string sscQuery = $"select * from ({baseQuery}) as temp where sBarcode in ( select sBarcode from StyleSizeCoversition where MainSBarcode='{sbarocde}' ) ";
                var converstionList = _dal.Select<StyleSize>(sscQuery, ref msg).ToList();
                data.itemsConvert = converstionList;

            }
            //dropdown list
            string itemListquery = baseQuery + " where SupID='" + data.SupID + "' and PrdID='" + data.PrdID + "' and IsConverationItem=0 ";
            var ObjItemList = _dal.Select<StyleSize>(itemListquery, ref msg).ToList();
            data.itemsList = ObjItemList;
            return data;
        }

        public List<StyleSize> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where CMPIDX like '%" + searchText + @"%' or ItemName like '%" + searchText + @"%'  
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<StyleSize>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where CMPIDX like '%" + searchText + @"%' or ItemName like '%" + searchText + @"%' ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [StyleSize] " + whereCluase;
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

        public bool Insert(StyleSize model)
        {
            model.isAuthorized = false;
            CompositeModel composite = new CompositeModel();
            composite.AddRecordSet<StyleSize>(model, OperationMode.Insert, "", "", "", "");
            if (model.itemsConvert != null)
            {
                InsertConvItemList(model, composite);
            }            
            model.CMPIDX = model.sBarcode + model.Barcode;
            model.ENTRYDT = DateTime.Now;

            bool response = _dal.InsertUpdateComposite(composite, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);
            return response;

        }

        public bool IsDataExist(StyleSize model)
        {
            query = baseQuery + $" where (CMPIDX<>'{model.CMPIDX}' and '{model.CMPIDX}' <>'' ) and (ItemName='{model.ItemName}' ) ";
            var response = _dal.SelectFirstOrDefault<StyleSize>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (response == null)
                return false;
            return true;
        }
        public string GenerateSBarcode(string SupID, string PrdID)
        {
            string prefix = SupID + PrdID; // 
            string generatedSbarCode = GetMaxIdWithPrfix2("sBarcode", "0000", "0001", "StyleSize", prefix);
            return generatedSbarCode;
        }
        public string GenerateBarcode()
        {
            string generatedSbarCode = GetMaxIdWithPrfix2("Barcode", "000000", "000001", "StyleSize", "");
            return generatedSbarCode;
        }
        public List<StyleSize> ItemDDL(string SupID, string PrdID)
        {
            //dropdown list
            string itemListquery = "select *from StyleSize where SupID='" + SupID + "' and PrdID='" + PrdID + "' and IsConverationItem=0 ";
            var ObjItemList = _dal.Select<StyleSize>(itemListquery, ref msg).ToList();
            return ObjItemList;
        }

        public List<StyleSize> SelectAll()
        {
            query = baseQuery;
            var response = _dal.Select<StyleSize>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return response;
        }

        public StyleSize StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var query = baseQuery + " where sBarcode='" + sbarocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }
        public StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            var query = baseQuery + " where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }
        public StyleSize ApprovedItembyBarcode(string barocde)
        {
            var query = baseQuery + " where Barcode='" + barocde + "' AND isAuthorized=1";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        public bool Update(StyleSize model)
        {
            CompositeModel composite = new CompositeModel();
            InsertConvItemList(model, composite);
            composite.AddRecordSet<StyleSize>(model, OperationMode.Update, "", "", "sBarcode", "");
            bool response = _dal.InsertUpdateComposite(composite, ref msg);
            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);
            return response;
        }

        private bool InsertConvItemList(StyleSize model, CompositeModel composite)
        {
            string mainBarcode = model.Barcode;
            string mainSbarcode = model.sBarcode;
            List<StyleSizeCoversition> lstStyleSizeConv = new List<StyleSizeCoversition>();
            if(model.itemsConvert != null)
            {
                foreach (var d in model.itemsConvert)
                {
                    StyleSizeCoversition sc = new StyleSizeCoversition();
                    sc.Barcode = d.Barcode;
                    sc.MainBarcode = mainBarcode;
                    sc.MainSBarcode = mainSbarcode;
                    sc.sBarcode = d.sBarcode;
                    lstStyleSizeConv.Add(sc);

                }
                composite.AddRecordSet<StyleSizeCoversition>(lstStyleSizeConv, OperationMode.Insert, "ConversitionId", "", "", "");
            }


            string sscQuery = $"( select sBarcode from StyleSizeCoversition where MainSBarcode='{model.sBarcode}' ) ";
            var converstionList = _dal.SelectFirstOrDefault<StyleSize>(sscQuery, ref msg);
            if (converstionList != null)
            {
                StyleSizeCoversition styleSizeCoversition = new StyleSizeCoversition { MainSBarcode = mainSbarcode };
                composite.AddRecordSet<StyleSizeCoversition>(styleSizeCoversition, OperationMode.Delete, "", "", "MainSBarcode", "");
            }
            
            return true;
        }

        public bool UpdateAuth(StyleSize model)
        {         
           return  _dal.Update<StyleSize>(model, "isAuthorized", "sBarcode", "", ref msg);          
        }
    }
}
