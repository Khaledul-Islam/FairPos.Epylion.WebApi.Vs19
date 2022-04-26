using FairPos.Epylion.Models.Requisition;
using FairPos.Epylion.Service.Requisition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RequisitionToPoController : ControllerBase
    {
        private readonly IRequisitionToPoService _service;

        public RequisitionToPoController(IRequisitionToPoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult RequisitionToPo()
        {
            var response = _service.GetNonPoRequesitionNo();
            if (response == null)
            {
                return BadRequest("No data found");
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetTempDataByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.GetTempDataByUser(userId);
            if (response == null)
            {
                return BadRequest("No data found");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetChallanDetails(string chlnNo)
        {
            if (string.IsNullOrEmpty(chlnNo))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.GetChallanDetails(chlnNo);
            if (response == null)
            {
                return BadRequest("No data found");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetsBuyOrderReqTemp(string chln)
        {
            if (string.IsNullOrEmpty(chln))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.GetsBuyOrderReqTemp(chln);
            if (response == null)
            {
                return BadRequest("No data found");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult LoadRequisition(string UserID, string shopID, string chlnn)
        {
            if (string.IsNullOrEmpty(chlnn))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.LoadRequisition(UserID, shopID, chlnn, out string errMsg);
            if (response == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult LoadRequisitionAll(string UserID, string shopID, string month,string supID)
        {
            if (string.IsNullOrEmpty(month))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.LoadRequisitionAll(UserID, shopID, month,supID, out string errMsg);
            if (response == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult UpdateRPUDeliveryDate(string barcode, string userid, decimal rpu, DateTime delDate)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.UpdateRPUDeliveryDate(barcode, userid, rpu, delDate);
            if (response == false)
            {
                return BadRequest("Error while update");
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult RemoveTmpRequisition(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.RemoveTmpRequisition(UserID);
            if (response == false)
            {
                return BadRequest("Error while update");
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveRequisitionToPO(List<BuyOrderReqTemp> olist)
        {
            if (olist==null)
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.SaveRequisitionToPO(olist, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
    }
}
