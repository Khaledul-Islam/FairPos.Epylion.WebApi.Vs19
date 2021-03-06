using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Requisition;
using FairPos.Epyllion.Repository.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Requisition
{
    public interface IRequisitionToPoService
    {
        List<AutoRequistion> GetNonPoRequesitionNo();
        List<BuyOrderReqTemp> GetTempDataByUser(string userId);
        List<AutoRequistion> GetChallanDetails(string chlnNo);
        IEnumerable<BuyOrderReqTemp> GetsBuyOrderReqTemp(string chln);
        bool LoadRequisition(string UserID, string shopID, string chlnn, out string errMsg);
        bool LoadRequisitionAll(string UserID, string shopID, string month, string supID, out string errMsg);
        bool UpdateRPUDeliveryDate(string barcode, string userid, decimal rpu, DateTime delDate);
        bool RemoveTmpRequisition(string UserID);
        string SaveRequisitionToPO(List<BuyOrderReqTemp> olist, out string errMsg);
    }
    public class RequisitionToPoService : IRequisitionToPoService
    {
        private readonly IRequisitionToPoRepository _repo;

        public RequisitionToPoService(IRequisitionToPoRepository repo)
        {
            _repo = repo;
        }

        public List<AutoRequistion> GetChallanDetails(string chlnNo)
        {
            return _repo.GetChallanDetails(chlnNo);
        }

        public List<AutoRequistion> GetNonPoRequesitionNo()
        {
            return _repo.GetNonPoRequesitionNo();
        }

        public IEnumerable<BuyOrderReqTemp> GetsBuyOrderReqTemp(string chln)
        {
            return _repo.GetsBuyOrderReqTemp(chln);
        }

        public List<BuyOrderReqTemp> GetTempDataByUser(string userId)
        {
            return _repo.GetTempDataByUser(userId);
        }

        public bool LoadRequisition(string UserID, string shopID, string chlnn, out string errMsg)
        {
            return _repo.LoadRequisition(UserID, shopID,chlnn, out errMsg);
        }
        public bool LoadRequisitionAll(string UserID, string shopID, string month, string supID, out string errMsg)
        {
            return _repo.LoadRequisitionAll(UserID, shopID, month,supID, out errMsg);
        }

        public bool RemoveTmpRequisition(string UserID)
        {
            return _repo.RemoveTmpRequisition(UserID);
        }

        public string SaveRequisitionToPO(List<BuyOrderReqTemp> olist, out string errMsg)
        {
            return _repo.SaveRequisitionToPO(olist, out errMsg); ;
        }

        public bool UpdateRPUDeliveryDate(string barcode, string userid, decimal rpu, DateTime delDate)
        {
            return _repo.UpdateRPUDeliveryDate(barcode, userid, rpu, delDate);
        }
    }
}
