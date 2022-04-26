using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FIK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FairPos.Epyllion.Repository.Operations
{
    public interface ICircularPriceChangedRepositroy
    {
        List<CircularPriceChangedDetail> GetCircularDetails(string cpno);
        List<CircularPriceChanged> GetPending();
        bool saveCircularPriceChangeApprove(List<CircularPriceChangedDetail> models, out string errMsg);
        bool saveTempPriceChanged(Buy buy);
        bool savePriceChanged(List<TempPriceChanged> tempData, out string errMsg);
        bool deleteTempPriceChanged(TempPriceChanged model);
        List<TempPriceChanged> GetTempDataByUser(string userId);

    }

    public class CircularPriceChangedRepositroy : BaseRepository, ICircularPriceChangedRepositroy
    {
        private readonly IItemConversionRepository _itemConv;
        public CircularPriceChangedRepositroy(IDBConnectionProvider dBConnectionProvider, IItemConversionRepository itemConv) : base(dBConnectionProvider)
        {
            //baseQuery = @"SELECT Id,CPCNo,CPCName,EffectiveDate,UserId,CreateDate,IsActivated,IsApproved,CASE WHEN (ROW_NUMBER() OVER (ORDER BY id)  =1)
            //            THEN (SELECT COUNT(*) FROM CircularPriceChanged) ELSE 0 END RecordCount FROM CircularPriceChanged";
            baseQuery = @"SELECT Id,CPCNo,CPCName,EffectiveDate,UserId,CreateDate,IsActivated,IsApproved FROM CircularPriceChanged";
            _itemConv = itemConv;
        }

        public bool deleteTempPriceChanged(TempPriceChanged model)
        {
            var resp = _dal.Delete<TempPriceChanged>(model, "UserId,Barcode", "", ref msg);
            return resp;
        }

        public List<CircularPriceChangedDetail> GetCircularDetails(string cpno)
        {
            string query = "SELECT c.*,s.ItemFullName as Description FROM CircularPriceChangedDetail c INNER JOIN vStyleSize s ON s.sBarcode = c.sBarcode WHERE CPCNo='" + cpno + "'";
            var response = _dal.Select<CircularPriceChangedDetail>(query, ref msg);
            return response;
        }

        public List<CircularPriceChanged> GetPending()
        {
            string query = baseQuery + " Where IsApproved = 0";

            var data = _dal.Select<CircularPriceChanged>(query, ref msg);
            return data;
        }
        public List<TempPriceChanged> GetTempDataByUser(string userId)
        {
            string query = "select * from TempPriceChanged where UserId='" + userId + "'";
            var respnse = _dal.Select<TempPriceChanged>(query, ref msg);
            return respnse;
        }

        public bool savePriceChanged(List<TempPriceChanged> tempData, out string errMsg)
        {
            errMsg = string.Empty;
            CompositeModel composite = new();
            if (tempData == null || tempData.Count == 0)
            {
                errMsg = "No item for select";
                return false;
            }
            string chln = GetMaxIdWithPrfix2("CPCNo", "0000000", "0000001", "CircularPriceChanged", "PC");
            string cols = "sBarcode,Barcode,CPCNo,CngRPU,RPU,CngAmount,EffectedbalQty,CngPrcnt,Status";
            foreach (var d in tempData)
            {
                CircularPriceChangedDetail details = new();
                details.sBarcode = d.sBarcode;
                details.Barcode = d.Barcode;
                details.CPCNo = chln;
                details.CngRPU = d.CngRPU;
                details.RPU = d.RPU;
                details.CngAmount = d.CngAmount;
                details.EffectedbalQty = d.EffectedbalQty;
                details.CngPrcnt = 0;
                details.Status = d.Status;
                composite.AddRecordSet<CircularPriceChangedDetail>(details, OperationMode.InsertOrUpdaet, "DetailsId", cols, "CPCNo,Barcode,sBarcode", "");
            }

            CircularPriceChanged circular = new();
            circular.CPCNo = chln;
            circular.EffectiveDate = DateTime.Now.Date;
            circular.CreateDate = DateTime.Now.Date;
            circular.UserId = tempData.First().UserId;
            circular.IsActivated = "N";
            circular.IsApproved = false;
            composite.AddRecordSet<CircularPriceChanged>(circular, OperationMode.Insert, "Id", "", "", "");
            composite.AddRecordSet<TempPriceChanged>(tempData.First(), OperationMode.Delete, "Id", "", "UserId", "");

            var response = _dal.InsertUpdateComposite(composite, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }

        public bool saveTempPriceChanged(Buy buy)
        {
            var SelectedItem = _itemConv.GetByBarcodeExp(buy.BarCode);
            TempPriceChanged otemp = new();
            otemp.Barcode = buy.BarCode;
            otemp.RPU = SelectedItem.RPU;
            otemp.CngRPU = buy.RPU;
            otemp.sBarcode = SelectedItem.sBarCode;
            otemp.UserId = buy.UserID;
            otemp.ProductName = buy.ProductDescription;
            otemp.EffectedbalQty = SelectedItem.balQty;
            if (otemp.RPU > otemp.CngRPU)
            {
                otemp.CngAmount = otemp.RPU - otemp.CngRPU;
                otemp.Status = "Decrease";
            }
            else
            {
                otemp.CngAmount = otemp.CngRPU - otemp.RPU;
                otemp.Status = "Increase";
            }
            otemp.CngPrcnt = 0;

            CompositeModel composite = new();
            composite.AddRecordSet<TempPriceChanged>(otemp, OperationMode.InsertOrUpdaet, "Tempid", "", "UserId,Barcode", "");
            var response = _dal.InsertUpdateComposite(composite, ref msg);
            return response;
        }

        public bool saveCircularPriceChangeApprove(List<CircularPriceChangedDetail> models, out string errMsg)
        {
            errMsg = string.Empty;
            List<string> query = new List<string>();
            foreach (var model in models)
            {
                StringBuilder sb = new();
                sb.AppendLine(" DECLARE @ChangeRPU MONEY = " + model.CngRPU + "; ");
                sb.AppendLine(" DECLARE @Barcode NVARCHAR(50) = '" + model.Barcode + "';");
                sb.AppendLine(" DECLARE @sBarcode NVARCHAR(50) = '" + model.sBarcode + "';");
                sb.AppendLine(" IF EXISTS(SELECT ISNULL(balQty,0) balQty FROM dbo.Buy WHERE BarCode = @Barcode)");
                sb.AppendLine(" BEGIN");
                sb.AppendLine(" UPDATE Buy SET RPU = @ChangeRPU WHERE BarCode = @Barcode;");
                sb.AppendLine(" UPDATE CircularPriceChangedDetail SET EffectedbalQty = EffectedbalQty + (SELECT balQty FROM Buy WHERE BarCode = @Barcode) WHERE Barcode = @Barcode");
                sb.AppendLine(" END");
                sb.AppendLine(" IF EXISTS(SELECT ISNULL(balQty, 0) balQty FROM dbo.BuyStaff WHERE BarCode = @Barcode)");
                sb.AppendLine(" BEGIN");
                sb.AppendLine(" UPDATE BuyStaff SET RPU = @ChangeRPU WHERE BarCode = @Barcode;");
                sb.AppendLine(" UPDATE CircularPriceChangedDetail SET EffectedbalQty = EffectedbalQty + (SELECT balQty FROM BuyStaff WHERE BarCode = @Barcode) WHERE Barcode = @Barcode");
                sb.AppendLine(" END");
                sb.AppendLine(" IF EXISTS(SELECT ISNULL(balQty, 0) balQty FROM dbo.BuyWorker WHERE BarCode = @Barcode)");
                sb.AppendLine(" BEGIN");
                sb.AppendLine(" UPDATE BuyWorker SET RPU = @ChangeRPU WHERE BarCode = @Barcode;");
                sb.AppendLine(" UPDATE CircularPriceChangedDetail SET EffectedbalQty = EffectedbalQty + (SELECT balQty FROM BuyWorker WHERE BarCode = @Barcode) WHERE Barcode = @Barcode");
                sb.AppendLine(" END");
                sb.AppendLine(" IF EXISTS(SELECT Barcode FROM dbo.StyleSize WHERE sBarCode = @sBarcode)");
                sb.AppendLine(" BEGIN");
                sb.AppendLine(" UPDATE StyleSize SET RPU = @ChangeRPU WHERE sBarcode = @sBarcode");
                sb.AppendLine(" END ");
                query.Add(sb.ToString());
            }
            string q = "UPDATE CircularPriceChanged SET IsActivated = 'Y', IsApproved = 1 WHERE CPCNo = '" + models.First().CPCNo + "'";
            query.Add(q);
            var response = _dal.ExecuteQuery(query, ref msg);
            if (response == false || !string.IsNullOrEmpty(msg))
            {
                errMsg = msg;
                return false;
            }
            return response;
        }


    }
}
