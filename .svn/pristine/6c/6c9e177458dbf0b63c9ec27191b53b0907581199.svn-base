using FairPos.Epylion.Models.Requisition;
using FairPos.Epylion.Service.Requisition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Requisition
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RequisitionApprovalController : ControllerBase
    {
        private readonly IRequisitionApprovalService _service;

        public RequisitionApprovalController(IRequisitionApprovalService service)
        {
            _service = service;
        }
        #region  RequisitionApproval
        [HttpGet]
        public IActionResult GetBychln(string chid)
        {
            if (string.IsNullOrEmpty(chid))
            {
                return BadRequest("Invalid Request");
            }
            var respopnse = _service.GetBychln(chid, out string errMsg);
            if (respopnse == null || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(respopnse);
        }
        [HttpGet]
        public IActionResult GetNonApprovedRequistion(string status, string shopid)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid Request");
            }
            if (string.IsNullOrEmpty(shopid))
            {
                return BadRequest("Invalid Request");
            }
            var respopnse = _service.GetNonApprovedRequistion(status, shopid, out string errMsg);
            if (respopnse == null || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(respopnse);
        }
        [HttpGet]
        public IActionResult AutoRequistionsApprovalLoad(string shopid)
        {
            if (shopid == null || shopid == "")
            {
                return BadRequest("No ShopID Found");
            }
            var respopnse = _service.AutoRequistionsApprovalLoad(shopid, out string errMsg);
            if (respopnse == null || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(respopnse);
        }

        [HttpPost]
        public IActionResult SaveApproveRequisition(List<AutoRequistion> olist)
        {
            var respopnse = _service.SaveApproveRequisition(olist, out string errMsg);
            if (respopnse == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(respopnse);
        }
        [HttpPost]
        public IActionResult RequisitionReject(List<AutoRequistion> olist)
        {
            var respopnse = _service.RequisitionReject(olist, out string errMsg);
            if (respopnse == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(respopnse);
        }
        #endregion

        #region RequisitionPOApproval
        [HttpGet]
        public IActionResult TempPODDL()
        {
            var response = _service.TempPODDL();
            if (response == null || response.Count == 0)
            {
                return BadRequest("No item found");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetChallanDetails(string tempPoNo)
        {
            if (tempPoNo == null)
            {
                return BadRequest();
            }
            var response = _service.GetChallanDetails(tempPoNo);
            if (response == null || response.Count == 0)
            {
                return BadRequest("No item found");
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult saveBuyOrderPOApproval(List<BuyOrderPriceApproval> lstbuy)
        {
            if (lstbuy == null)
            {
                return BadRequest();
            }
            var res = _service.saveBuyOrderPOApproval(lstbuy, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(res);
        }
        #endregion
    }
}
