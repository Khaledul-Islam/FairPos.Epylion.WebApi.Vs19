using Dapper;
using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epyllion.Repository.Operations
{
    public interface IPurchaseOrderRepository
    {
        bool Insert(BuyOrderTemp model);
        string PurchaseOrder(List<BuyOrderTemp> model);
        bool RemoveBuyOrderTemp(BuyOrderTemp model);
        bool RemoveBuyOrderTempByID(string barCode);
        decimal GetActualStockBySbarocde(string sBarcode);
        List<BuyOrderTemp> GetBuyOrderTemps(string UserID);
        List<BuyOrderTemp> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null);
        List<BuyOrderTempEdit> GetsForDataTablesPOEdit(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null);
        //PO EDIT
        List<string> GetPONotReceived(string supplierId);
        bool POSaveTemp(string pono, string userid, string shopid, out string errMsg);
        bool DeletePOEditAll(string userid, string shopid, string chln, out string errMsg);
        string PurchaseOrderEditSave(List<BuyOrderTemp> model, out string errMsg);
        bool RemoveBuyOrderTempEditByID(string barCode);
        bool updateTmpPurchaseEdit(BuyOrderTempEdit item, out string errMsg);
        bool InsertPOEdit(BuyOrderTemp model);
    }

    public class PurchaseOrderRepository : BaseRepository, IPurchaseOrderRepository
    {
        private readonly IEmailHelperRepository _emailHelperService;
        private readonly ISupplierRepository _supplierService;
        public PurchaseOrderRepository(IDBConnectionProvider dBConnectionProvider, IEmailHelperRepository emailHelperService, ISupplierRepository supplierService) : base(dBConnectionProvider)
        {
            baseQuery = @"SELECT [CmpIDX]
                              ,[Chln]
                              ,[SupID]
                              ,[sBarCode]
                              ,[BarCode]
                              ,[BoxQty]
                              ,[Qty]
                              ,[sQty]
                              ,[DiscPrcnt]
                              ,[VATPrcnt]
                              ,[PrdComm]
                              ,[CPU]
                              ,[RPU]
                              ,[BuyDT]
                              ,[EXPDT]
                              ,[UserID]
                              ,[PrdDescription]
                              ,[BoxUOM]
                              ,[UnitUOM]
                              ,[DeliveryDate]
                              ,[PrdID]
                              ,[POPackQty]
                              ,[PackUOM]
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY CmpIDX)  =1)
	                     THEN	(SELECT COUNT(*) FROM BuyOrderTemp) 
	                     ELSE 0 END RecordCount
                    from BuyOrderTemp";
            _emailHelperService = emailHelperService;
            _supplierService = supplierService;
        }
        public string PurchaseOrder(List<BuyOrderTemp> model)
        {
            try
            {
                string chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "BuyOrder", "PO/" + DateTime.Now.Year.ToString() + "/" + model[0].ShopID + "/");
                CompositeModel composite = new();
                foreach (var d in model)
                {
                    BuyOrder sts = new();
                    sts.BarCode = d.BarCode;
                    sts.sBarCode = d.sBarCode;
                    sts.Chln = chln;
                    sts.CmpIDX = sts.Chln + sts.BarCode + DateTime.Now.Millisecond.ToString() + d.sBarCode.ToString() + DateTime.Now.Millisecond.ToString();
                    sts.CPU = d.CPU;
                    sts.Qty = d.Qty;
                    sts.BoxQty = d.BoxQty;
                    sts.POPackQty = d.POPackQty;
                    sts.BuyDT = DateTime.Now;
                    sts.EXPDT = d.DeliveryDate;//conf
                    sts.RPU = d.RPU;
                    sts.SupID = d.SupID;
                    sts.PrdComm = d.PrdComm;
                    sts.DiscPrcnt = d.DiscPrcnt;
                    sts.VATPrcnt = 0;
                    sts.sQty = d.sQty;
                    sts.SupID = d.SupID;
                    sts.UserID = d.UserID;
                    sts.IS_ARRIVAL = false;
                    sts.IS_CANCEL = false;
                    sts.VatAmt = 0;
                    sts.QutRefNo = d.QtrefNo;
                    sts.PartialDelivery = d.PartialDelivery;
                    sts.MaturtyDays = d.MaturityDaysID.ToString();
                    sts.PaymentTerms = d.PaymentTermName;
                    sts.ShopID = d.ShopID;
                    composite.AddRecordSet<BuyOrder>(sts, OperationMode.Insert, "", "", "", "");
                }
                BuyOrderTemp b = new();
                b.UserID = model.First().UserID;
                b.ShopID = model.First().ShopID;
                composite.AddRecordSet<BuyOrderTemp>(b, OperationMode.Delete, "", "", "UserID,ShopID", "");

                var res = _dal.InsertUpdateComposite(composite, ref msg);

                if (res)
                {
                    Supplier aSupplier = _supplierService.FindById(model.FirstOrDefault().SupID);
                    List<string> lst = new();
                    lst.Add(aSupplier.RegEmail);
                    _emailHelperService.EmailSender("Ordered !!!", lst, new List<string>(), "Ordered !!!", null, false);
                    return chln;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Insert(BuyOrderTemp model)
        {
            string Query = "select ShopID from UsersShop where UserId='" + model.UserID + "'";
            string shopID = _dal.Select<UsersShop>(Query, ref msg).FirstOrDefault().ShopID;
            if (model.isAdd == true && model.isEdit == false)
            {
                string chln = GetMaxIdWithPrfix2("RequisitionNo", "000000", "000001", "BuyRequisition", shopID);//shopid HM13 from seaasion also userid from session
                model.Chln = chln;
                model.DeliveryDate = model.DeliveryDate.Date;
                model.CmpIDX = chln + model.BarCode.ToString() + model.DeliveryDate;
                model.BoxQty = model.Qty / model.BoxSize;
                model.BuyDT = DateTime.Now;
                model.EXPDT = model.DeliveryDate;
                model.BoxUOM = model.PackUOM;
                model.PrdComm = 0;
                model.VATPrcnt = 0;
                model.DiscPrcnt = 0;
                model.sQty = 0;

                bool response = _dal.Insert<BuyOrderTemp>(model, "", "", "", ref msg);

                return response;
            }
            else
            {
                query = baseQuery + $" where BarCode ='{model.BarCode}' AND sBarCode ='{model.sBarCode}' AND DeliveryDate ='{model.DeliveryDate}' ";
                var obj = _dal.SelectFirstOrDefault<BuyOrderTemp>(query, ref msg);
                bool response = _dal.Delete<BuyOrderTemp>(obj, "BarCode,sBarCode,DeliveryDate", "", ref msg);
                if (response)
                {
                    string chln = GetMaxIdWithPrfix2("RequisitionNo", "000000", "000001", "BuyRequisition", shopID);//shopid HM13 from seaasion also userid from session
                    model.Chln = chln;
                    model.CmpIDX = chln + model.BarCode + model.DeliveryDate;
                    model.BoxQty = model.Qty / model.BoxSize;
                    model.BuyDT = DateTime.Now;
                    model.EXPDT = model.DeliveryDate;
                    model.BoxUOM = model.PackUOM;
                    model.PrdComm = 0;
                    model.VATPrcnt = 0;
                    model.DiscPrcnt = 0;
                    model.sQty = 0;

                    bool res = _dal.Insert<BuyOrderTemp>(model, "", "", "", ref msg);

                    if (!string.IsNullOrEmpty(msg))
                        throw new Exception(msg);

                    return res;
                }
                return response;
            }
        }
        private StyleSize StyleSizeDetailsByBarcode(string barocde)
        {
            string query = "SELECT * FROM vStyleSize where Barcode='" + barocde + "'";
            var data = _dal.Select<StyleSize>(query, ref msg).FirstOrDefault();
            return data;
        }

        public List<BuyOrderTemp> GetBuyOrderTemps(string UserID)
        {
            query = baseQuery + $" where UserID='{UserID}'";
            var response = _dal.Select<BuyOrderTemp>(query, ref msg).ToList();

            return response;
        }

        public List<BuyOrderTemp> GetsForDataTables(int start, int length, string orderBy, ref string msgs, string UserID, string searchText = null)
        {
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserID='" + UserID + "' AND ( BarCode like '%" + searchText + @"%' or PrdDescription like '%" + searchText + @"%') 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = " where UserID='" + UserID + "'" + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQuery, whereCluase);
            var data = _dal.Select<BuyOrderTemp>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where UserID='" + UserID + "' AND (BarCode like '%" + searchText + @"%' or PrdDescription like '%" + searchText + @"%') ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [BuyOrderTemp] " + whereCluase;
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

        public List<BuyOrderTempEdit> GetsForDataTablesPOEdit(int start, int length, string orderBy, ref string msgs, string UserID, string ShopID, string searchText = null)
        {
            string baseQueryedit = @"SELECT *
                        ,CASE WHEN (ROW_NUMBER() OVER (ORDER BY CmpIDX)  =1)
	                     THEN	(SELECT COUNT(*) FROM BuyOrderTempEdit) 
	                     ELSE 0 END RecordCount
                    from BuyOrderTempEdit";
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where ShopID='" + ShopID + "' AND UserID='" + UserID + "' AND ( BarCode like '%" + searchText + @"%' or PrdDescription like '%" + searchText + @"%') 
                       " + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }
            else
            {
                whereCluase = " where ShopID='" + ShopID + "' AND UserID='" + UserID + "'" + orderBy + "  OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY";
            }

            query = string.Format("{0} {1}", baseQueryedit, whereCluase);
            var data = _dal.Select<BuyOrderTempEdit>(query, ref msg);

            #region filter data count
            whereCluase = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                whereCluase = " where ShopID='" + ShopID + "' AND UserID='" + UserID + "' AND (BarCode like '%" + searchText + @"%' or PrdDescription like '%" + searchText + @"%') ";
            }
            else
            {
                whereCluase = "  ";
            }

            query = " SELECT COUNT(*) FROM [BuyOrderTempEdit] " + whereCluase;
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

        public bool RemoveBuyOrderTemp(BuyOrderTemp model)
        {
            query = baseQuery + $" where UserID='{model.UserID}' AND BarCode='{model.BarCode}' ";
            var obj = _dal.SelectFirstOrDefault<BuyOrderTemp>(query, ref msg);
            if (obj == null)
            {
                return false;
            }
            bool response = _dal.Delete<BuyOrderTemp>(obj, "SupID", "", ref msg);
            return response;
        }
        public decimal GetActualStockBySbarocde(string sBarcode)
        {
            decimal sum = 0;
            string query = "select sum(balQty) as balQty from buy where sBarCode='" + sBarcode + "'";
            var response = _dal.SelectFirstColumn(query, ref msg);
            decimal.TryParse(response, out sum);
            return sum;
        }
        public bool RemoveBuyOrderTempByID(string barCode)
        {
            query = baseQuery + $" where BarCode='{barCode}' ";
            var obj = _dal.SelectFirstOrDefault<BuyOrderTemp>(query, ref msg);
            if (obj == null)
            {
                return false;
            }
            bool response = _dal.Delete<BuyOrderTemp>(obj, "BarCode", "", ref msg);
            return response;
        }


        public bool POSaveTemp(string pono, string userid, string shopid, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            List<BuyOrder> orders = _dal.Select<BuyOrder>("select * from BuyOrder where Chln='" + pono + "'", ref msg);
            if (orders.Count == 0)
            {
                errMsg = "No order found";
                return false;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }

            List<BuyOrderTempEdit> arrivals = _dal.Select<BuyOrderTempEdit>("select * from BuyOrderTempEdit", ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            if (arrivals.Count > 0)
            {
                errMsg = "A PO is already processing by another user";
                return false;
            }

            string cols = @"BarCode,POPackSize,BoxSize,RPU,CPU,CmpIDX,sBarCode,+Qty,Chln,+BoxQty,+POPackQty,UnitUOM,UserID,ShopID,PrdDescription,BuyDT,EXPDT,BoxUOM,PackUOM,DeliveryDate,PrdComm,VATPrcnt,DiscPrcnt,sQty,SupID";

            foreach (BuyOrder d in orders)
            {
                var SelectedItem = StyleSizeDetailsByBarcode(d.BarCode);
                BuyOrderTempEdit otemp = new BuyOrderTempEdit();
                otemp.BarCode = d.BarCode;
                otemp.RPU = d.RPU;
                otemp.CPU = d.CPU;
                otemp.CmpIDX = d.CmpIDX;
                otemp.sBarCode = d.sBarCode;
                otemp.Qty = d.Qty;
                otemp.Chln = d.Chln;
                otemp.BoxQty = d.BoxQty;
                otemp.POPackQty = d.POPackQty;
                otemp.UserID = userid;
                otemp.ShopID = shopid;
                otemp.PrdDescription = SelectedItem.ItemFullName;
                otemp.BuyDT = d.BuyDT;
                otemp.EXPDT = d.EXPDT;
                otemp.BoxUOM = SelectedItem.PackUiomName;
                otemp.UnitUOM = SelectedItem.BOXUOMName;
                otemp.PackUOM = SelectedItem.PackUiomName;
                otemp.BoxSize = SelectedItem.BoxSize;
                otemp.POPackSize = SelectedItem.POPackSize;
                otemp.DeliveryDate = d.EXPDT;
                otemp.PrdComm = 0;
                otemp.VATPrcnt = 0;
                otemp.DiscPrcnt = 0;
                otemp.sQty = 0;
                otemp.SupID = SelectedItem.SupID;
                composite.AddRecordSet<BuyOrderTempEdit>(otemp, OperationMode.InsertOrUpdaet, "", cols, "UserID,sBarCode,ShopID", "");
            }

            var res = _dal.InsertUpdateComposite(composite, ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return res;
        }
        public List<string> GetPONotReceived(string supplierId)
        {
            string query = @"SELECT DISTINCT Chln 
                            FROM dbo.BuyOrder
                            WHERE SupID='" + supplierId + @"'
                            AND Chln NOT IN ( SELECT DISTINCT Chln FROM dbo.Arrival ) ";
            return _dapper.Query<string>(query).ToList();

        }

        public bool DeletePOEditAll(string userid, string shopid, string chln, out string errMsg)
        {
            errMsg = string.Empty;
            BuyOrderTempEdit b = new();
            b.UserID = userid;
            b.ShopID = shopid;
            b.Chln = chln;

            var res = _dal.Delete<BuyOrderTempEdit>(b, "Chln,ShopID,UserID", "", ref msg);
            if (!string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return res;
        }
        public string RemovePO(string poNO)
        {
            var data = _dal.Select<BuyOrder>("select * from BuyOrder where Chln='" + poNO + "'", ref msg);

            if (data.Count == 0 || data == null)
            {
                return "";
            }
            foreach (var d in data)
            {
                if (d.IS_ARRIVAL == true || d.IS_CANCEL == true)
                {
                    return "Received";
                }
            }
            var res = _dal.Delete<BuyOrder>(data.First(), "Chln", "", ref msg);
            if (res == true)
            {
                return "Deleted";
            }
            else
            {
                return "Error";
            }

        }
        public string PurchaseOrderEditSave(List<BuyOrderTemp> model, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            try
            {
                string chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "BuyOrder", "PO/" + DateTime.Now.Year.ToString() + "/" + model[0].ShopID + "/");
                var del = RemovePO(model.First().Chln);
                if (del == "Received")
                {
                    errMsg = "This PO Already Received";
                    return "false";
                }
                else if (del == "Error")
                {
                    errMsg = "Error Occured when delete the data";
                    return "false";
                }
                foreach (var d in model)
                {
                    BuyOrder sts = new();
                    sts.BarCode = d.BarCode;
                    sts.sBarCode = d.sBarCode;
                    sts.Chln = chln;
                    sts.CmpIDX = sts.Chln + sts.BarCode + DateTime.Now.Millisecond.ToString() + d.sBarCode.ToString() + DateTime.Now.Millisecond.ToString();
                    sts.CPU = d.CPU;
                    sts.Qty = d.Qty;
                    sts.BoxQty = d.BoxQty;
                    sts.POPackQty = d.POPackQty;
                    sts.BuyDT = DateTime.Now;
                    sts.EXPDT = d.DeliveryDate;//conf
                    sts.RPU = d.RPU;
                    sts.SupID = d.SupID;
                    sts.PrdComm = d.PrdComm;
                    sts.DiscPrcnt = d.DiscPrcnt;
                    sts.VATPrcnt = 0;
                    sts.sQty = d.sQty;
                    sts.SupID = d.SupID;
                    sts.UserID = d.UserID;
                    sts.IS_ARRIVAL = false;
                    sts.IS_CANCEL = false;
                    sts.VatAmt = 0;
                    sts.QutRefNo = d.QtrefNo;
                    sts.PartialDelivery = d.PartialDelivery;
                    sts.MaturtyDays = d.MaturityDaysID.ToString();
                    sts.PaymentTerms = d.PaymentTermName;
                    sts.ShopID = d.ShopID;
                    composite.AddRecordSet<BuyOrder>(sts, OperationMode.Insert, "", "", "", "");
                }
                BuyOrderTempEdit b = new();
                b.UserID = model.First().UserID;
                b.ShopID = model.First().ShopID;
                composite.AddRecordSet<BuyOrderTempEdit>(b, OperationMode.Delete, "", "", "UserID,ShopID", "");

                if (!string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                var res = _dal.InsertUpdateComposite(composite, ref msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    errMsg = msg;
                    return "false";
                }
                if (res)
                {
                    Supplier aSupplier = _supplierService.FindById(model.FirstOrDefault().SupID);
                    List<string> lst = new();
                    lst.Add(aSupplier.RegEmail);
                    _emailHelperService.EmailSender("Ordered !!!", lst, new List<string>(), "Ordered !!!", null, false);
                    return chln;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool RemoveBuyOrderTempEditByID(string barCode)
        {
            query = "select * from BuyOrderTempEdit where BarCode='" + barCode + "' ";
            var obj = _dal.SelectFirstOrDefault<BuyOrderTempEdit>(query, ref msg);
            if (obj == null)
            {
                return false;
            }
            bool response = _dal.Delete<BuyOrderTempEdit>(obj, "BarCode", "", ref msg);
            return response;
        }

        public bool updateTmpPurchaseEdit(BuyOrderTempEdit item, out string errMsg)
        {
            errMsg = string.Empty;
            var response = _dal.Update<BuyOrderTempEdit>(item, "DeliveryDate,Qty,POPackQty,RPU", "BarCode", "", ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }
        public bool InsertPOEdit(BuyOrderTemp model)
        {
            string cols = @"BarCode,POPackSize,BoxSize,RPU,CPU,CmpIDX,sBarCode,+Qty,Chln,+BoxQty,+POPackQty,UnitUOM,UserID,ShopID,PrdDescription,BuyDT,EXPDT,BoxUOM,PackUOM,DeliveryDate,PrdComm,VATPrcnt,DiscPrcnt,sQty,SupID";

            CompositeModel composite = new();
            string chln = GetMaxIdWithPrfix2("Chln", "0000", "0001", "BuyOrder", "PO/" + DateTime.Now.Year.ToString() + "/" + model.ShopID + "/");
            var SelectedItem = StyleSizeDetailsByBarcode(model.BarCode);
            BuyOrderTempEdit otemp = new();
            otemp.BarCode = model.BarCode;
            otemp.RPU = model.RPU;
            otemp.CPU = model.CPU;
            otemp.CmpIDX = model.Chln + model.BarCode + DateTime.Now.Millisecond.ToString() + SelectedItem.sBarcode.ToString() + DateTime.Now.Millisecond.ToString();
            otemp.sBarCode = SelectedItem.sBarcode;
            otemp.Chln = chln;
            otemp.Qty = model.Qty;
            otemp.BoxQty = model.Qty / model.BoxSize;
            otemp.POPackQty = model.POPackQty;
            otemp.UserID = model.UserID;
            otemp.ShopID = model.ShopID;
            otemp.PrdDescription = model.PrdDescription;
            otemp.BuyDT = DateTime.Now;
            otemp.EXPDT = model.DeliveryDate;
            otemp.BoxUOM = model.PackUOM;
            otemp.UnitUOM = model.UnitUOM;
            otemp.PackUOM = SelectedItem.PackUiomName;
            otemp.BoxSize = SelectedItem.BoxSize;
            otemp.POPackSize = SelectedItem.POPackSize;
            otemp.DeliveryDate = model.DeliveryDate;
            otemp.PrdComm = 0;
            otemp.VATPrcnt = 0;
            otemp.DiscPrcnt = 0;
            otemp.sQty = 0;
            otemp.SupID = SelectedItem.SupID;
            otemp.BoxUOM = model.PackUOM;
            otemp.ShopID = model.ShopID;

            composite.AddRecordSet<BuyOrderTempEdit>(otemp, OperationMode.InsertOrUpdaet, "", cols, "UserID,sBarCode,ShopID", "");
            bool response = _dal.InsertUpdateComposite(composite, ref msg);

            return response;
        }

    }
}
