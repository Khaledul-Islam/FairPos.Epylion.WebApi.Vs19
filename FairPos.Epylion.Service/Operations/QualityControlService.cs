using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epyllion.Repository.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Operations
{
    public interface IQualityControlService
    {
        List<Supplier> QCSupplierDDL(string ShopID, string UserID);
        List<Arrival> GetPendingPONo(string supplierId, string ShopID, string UserID);
        List<TempQualityControl> GetTempQualityControl(string UserID, string ShopID);
        List<TempQualityControl> GetTempQualityControlByChln(string chln);
        List<StyleSize> GetChallanItems(string dcno);
        List<Arrival> GetChallanDetails(string dcno, string sBarcode);
        bool SaveTempArrival(List<Arrival> model, out string errMsg);
        bool DeleteTempQC(string UserID, out string errMsg);
        List<string> SaveQualityControl(List<TempQualityControl> details, out string errMsg);
    }
    public class QualityControlService : IQualityControlService
    {
        private readonly IQualityControlRepository _repo;

        public QualityControlService(IQualityControlRepository repo)
        {
            _repo = repo;
        }

        public bool DeleteTempQC(string UserID, out string errMsg)
        {
            return _repo.DeleteTempQC(UserID,out errMsg);
        }

        public List<Arrival> GetChallanDetails(string dcno, string sBarcode)
        {
            return _repo.GetChallanDetails(dcno,sBarcode);
        }

        public List<StyleSize> GetChallanItems(string dcno)
        {
            return _repo.GetChallanItems(dcno);
        }

        public List<Arrival> GetPendingPONo(string supplierId, string ShopID, string UserID)
        {
            return _repo.GetPendingPONo(supplierId,ShopID,UserID);
        }

        public List<TempQualityControl> GetTempQualityControl(string UserID, string ShopID)
        {
            return _repo.GetTempQualityControl(UserID,ShopID);
        }

        public List<TempQualityControl> GetTempQualityControlByChln(string chln)
        {
            return _repo.GetTempQualityControlByChln(chln);
        }

        public List<Supplier> QCSupplierDDL(string ShopID, string UserID)
        {
            return _repo.QCSupplierDDL(ShopID,UserID);
        }

        public List<string> SaveQualityControl(List<TempQualityControl> details, out string errMsg)
        {
            return _repo.SaveQualityControl(details,out errMsg);
        }

        public bool SaveTempArrival(List<Arrival> model, out string errMsg)
        {
            return _repo.SaveTempArrival(model,out errMsg);
        }
    }
}
