using FairPos.Epylion.Models;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository
{

    public interface ISupplierRepository
    {
        bool Insert(Supplier model);
        bool Update(Supplier model);
        bool Delete(Supplier model);
        List<Supplier> SelectAll();
        Supplier FindById(string id);
        List<Supplier> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null);
        bool IsDataExist(Supplier model);
        SupplierDoc FindDocumentById(string id);
    }

    public class SupplierRepository : BaseRepository, ISupplierRepository
    {
        private string querySupplierDoc = "";

        public SupplierRepository(IDBConnectionProvider dBConnectionProvider) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT SupID,RegName,RegAdd1,RegAdd2,RegPhone,RegFax,RegWeb,RegEmail,ChkRegOwner,
                           ChkRegPartner,chkCashCheq,chkBEFTN,Supname,TradeAdd1,TradeAdd2,TradePhone,
                           TradeFax,TradeWeb,TradeEmail,chkTradeMember,chkTradeDirector,GenCName,
                           GenCDesig,GenCCell,GenCEmail,MgtCName,MgtCDesig,MgtCCell,MgtCEmail,
                           MngCname,MngCdesig,MngCCell,MngCEmail,FinCname,FinCDesig,FinCCell,
                           FinCEmail,gMargin,gMarginTP,gMarginAVG,chkgMarginAP,asDays,sacDays,
                           B2BDays,SupType,crDays,chkAC,chkPO,chkCash,IssueFor,Bank,BankBR,BankBRCode,
                           ACCName,ACCnum,ACCNB,ACCtype,ACCMNB,chkSupDay,chkSupWeek,chkSupMonth,chkSupAsPer,
                           DeliveryDays,TransportMode,chkDamageRep,chkDamageRet,chkSlowRep,chkSlowRet,
                           chkShortRep,chkShortRet,chkExpireRep,chkExpireRet,InformDay,chkTradeLicence,
                           chkBSTIdocument,chkVATCertificate,chkTINCertificate,chkOtherDocument,chkTypeLocal,
                           chkTypeForeign,chkTypeOther,TraderEMOSCode,SupArea,SupCommodity,SPonTP,SPonMRP,
                           SplDiscount,SupCategory,Fired,DOE,Address,Phone,MaturityDays	 
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY SupID)  =1)
	                     THEN	(SELECT COUNT(*) FROM Supplier) 
	                     ELSE 0 END RecordCount
                    FROM Supplier";

            querySupplierDoc = @"SELECT SupID,TradeFileNo,TradeFileType,TradeFile,BSTIFileType,BSTIFile
                                  ,BSTIFileNo,VatFileType,VatFile,VatFileNo,TinFileType,TinFile
                                  ,TinFileNo,OtherDocType,OtherDoc
                              FROM SupplierDoc";
        }

        public bool Insert(Supplier model)
        {

            model.SupID = GetMaxId("SupID", "0000", "0001", "Supplier");

            CompositeModel cm = new CompositeModel();
            cm.AddRecordSet<Supplier>(model, OperationMode.Insert, "", "", "", "");
            if(model.supplierDoc != null)
            {
                model.supplierDoc.SupID = model.SupID;
                cm.AddRecordSet<SupplierDoc>(model.supplierDoc, OperationMode.Insert, "", "", "", "");
            }

            bool r = _dal.InsertUpdateComposite(cm, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }

        public bool IsDataExist(Supplier model)
        {
            query = baseQuery + $" where (SupID<>'{model.SupID}' and '{model.SupID}' <>'' ) and (Supname='{model.Supname}' or RegName='{model.RegName}' ) ";
            var r = _dal.SelectFirstOrDefault<Supplier>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            if (r == null)
                return false;
            return true;

        }


        public bool Update(Supplier model)
        {
            bool r = false;


            //r = _dal.Update<Supplier>(model, "", "SupID", "", ref msg);

            CompositeModel cm = new CompositeModel();
            cm.AddRecordSet<Supplier>(model, OperationMode.Update, "", "", "SupID", "");
            if (model.supplierDoc != null)
            {
                cm.AddRecordSet<SupplierDoc>(model.supplierDoc, OperationMode.Update, "", "", "SupID", "");
            }

            r = _dal.InsertUpdateComposite(cm, ref msg);


            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public bool Delete(Supplier model)
        {

            //bool r = _dal.Delete<Supplier>(model, "SupID", "", ref msg);

            CompositeModel cm = new CompositeModel();
            cm.AddRecordSet<Supplier>(model, OperationMode.Update, "", "", "SupID", "");
            if (model.supplierDoc != null)
            {
                cm.AddRecordSet<SupplierDoc>(model.supplierDoc, OperationMode.Update, "", "", "SupID", "");
            }

            bool r = _dal.InsertUpdateComposite(cm, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public List<Supplier> SelectAll()
        {
            query = baseQuery;
            var r = _dal.Select<Supplier>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public Supplier FindById(string id)
        {
            query = baseQuery + $" where SupID='{id}' ";
            var r = _dal.SelectFirstOrDefault<Supplier>(query, ref msg);

            if (!string.IsNullOrEmpty(msg))
                throw new Exception(msg);

            return r;
        }


        public SupplierDoc FindDocumentById(string id)
        {
            var doc = _dal.SelectFirstOrDefault<SupplierDoc>(querySupplierDoc + $" where SupID='{id}' ", ref msg);
            return doc;
        }


        public List<Supplier> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string searchText = null)
        {
            whereCluase = "";
            offsetCluase = $"  OFFSET {start} ROWS FETCH NEXT {length} ROWS ONLY";

            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = $" where SupID like '%{searchText}%' or RegName like '%{searchText}%' or Supname like '%{searchText}%' or Phone like '%{searchText}%' or Address like '%{searchText}%'";
                      // " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            

            query = string.Format("{0} {1} {2} {3}", baseQuery, whereCluase, orderBy , offsetCluase);
            var data = _dal.Select<Supplier>(query, ref msg);

            #region filter data count
            

            query = " SELECT COUNT(*) FROM [Supplier] " + whereCluase;
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
