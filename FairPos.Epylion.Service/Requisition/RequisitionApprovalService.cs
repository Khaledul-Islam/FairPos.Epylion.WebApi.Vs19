using FairPos.Epylion.Models.Requisition;
using FairPos.Epyllion.Repository.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Service.Requisition
{
    public interface IRequisitionApprovalService
    {
        List<AutoRequistion> GetNonApprovedRequistion(string status, string shopid, out string errMsg);
        AutoRequistionTempApproval GetBychln(string id, out string errMsg);
        public List<AutoRequistion> AutoRequistionsApprovalLoad(string shopid, out string msgg);
        bool SaveApproveRequisition(List<AutoRequistion> olist, out string errMsg);
        bool RequisitionReject(List<AutoRequistion> olist, out string errMsg);
        //
        public List<string> TempPODDL();
        List<BuyOrderPriceApproval> GetChallanDetails(string tempPoNo);
        string saveBuyOrderPOApproval(List<BuyOrderPriceApproval> lstbuy, out string errMsg);
    }
    public class RequisitionApprovalService : IRequisitionApprovalService
    {
        private readonly IRequisitionApprovalRepository _repo;

        public RequisitionApprovalService(IRequisitionApprovalRepository repo)
        {
            _repo = repo;
        }

        public List<AutoRequistion> AutoRequistionsApprovalLoad(string shopid, out string msgg)
        {
            return _repo.AutoRequistionsApprovalLoad(shopid, out msgg);
        }

        public AutoRequistionTempApproval GetBychln(string id, out string errMsg)
        {
            return _repo.GetBychln(id, out errMsg);
        }

        public List<BuyOrderPriceApproval> GetChallanDetails(string tempPoNo)
        {
            return _repo.GetChallanDetails(tempPoNo);
        }

        public List<AutoRequistion> GetNonApprovedRequistion(string status, string shopid, out string errMsg)
        {
            return _repo.GetNonApprovedRequistion(status,shopid, out errMsg);
        }

        public bool RequisitionReject(List<AutoRequistion> olist, out string errMsg)
        {
            return _repo.RequisitionReject(olist, out errMsg);
        }

        public bool SaveApproveRequisition(List<AutoRequistion> olist, out string errMsg)
        {
            return _repo.SaveApproveRequisition(olist, out errMsg);
        }

        public string saveBuyOrderPOApproval(List<BuyOrderPriceApproval> lstbuy, out string errMsg)
        {
            return _repo.saveBuyOrderPOApproval(lstbuy, out errMsg);
        }

        public List<string> TempPODDL()
        {
            return _repo.TempPODDL();
        }
    }
}
