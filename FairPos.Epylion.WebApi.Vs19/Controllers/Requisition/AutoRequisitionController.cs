using FairPos.Epylion.Models.Requisition;
using FairPos.Epylion.Service.Requisition;
using Microsoft.AspNetCore.Authorization;
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
    // [Authorize(Policy = nameof(Policy.Account))]
    public class AutoRequisitionController : ControllerBase
    {
        private readonly IAutoRequisitionService _service;

        public AutoRequisitionController(IAutoRequisitionService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GenerateRequistion(string UserID, string ShopID)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest("NO User ID Found");
            }
            var response = _service.GenerateRequistion(UserID, ShopID, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == false)
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempRequistionDataByUser(string user,string shopid)
        {

            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("NO User ID Found");
            }
            if (string.IsNullOrEmpty(shopid))
            {
                return BadRequest("NO Shop ID Found");
            }
            var response = _service.GetTempRequistionDataByUser(user,shopid, out string errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return NotFound(errMsg);
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult RemoveAllTempRequistion(string userId,string shopid)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("NO User ID Found");
            }
            if (string.IsNullOrEmpty(shopid))
            {
                return BadRequest("NO Shop ID Found");
            }
            var response = _service.RemoveAllTempRequistion(userId,shopid, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == false)
            {
                return NotFound("No Item to Delete");
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult UpdateActualQty(AutoRequistionTemp model)
        {
            if (model == null)
            {
                return BadRequest("NO Item to Update");
            }
            var response = _service.UpdateActualQty(model);
            if (response == false)
            {
                return NotFound("No Item to Update");
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult AutoRequisitionSave(List<AutoRequistionTemp> olist)
        {
            if (olist.Count == 0)
            {
                return BadRequest("No Item to save");
            }
            var response = _service.AutoRequisitionSave(olist,out string errMsg);
            if (response == "false" || !string.IsNullOrEmpty(errMsg))
            {
                return NotFound(errMsg);
            }
            return Ok(response);
        }
    }
}
