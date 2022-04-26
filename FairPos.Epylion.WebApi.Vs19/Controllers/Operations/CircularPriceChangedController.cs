using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Operations
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class CircularPriceChangedController : ControllerBase
    {
        private readonly ICircularPriceChangedService _Service;

        public CircularPriceChangedController(ICircularPriceChangedService Service)
        {
            _Service = Service;
        }
        [HttpPost]
        public IActionResult saveCircularPriceChangeApprove(List<CircularPriceChangedDetail> model)
        {
            if (model == null)
            {
                return NotFound();
            }
            var response = _Service.saveCircularPriceChangeApprove(model, out string errMsg);
            if(response==false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
            
        [HttpGet]
        public IActionResult GetPending()
        {
            var response = _Service.GetPending();
            if (response == null)
            {
                return BadRequest("response null");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetCircularDetails(string cpno)
        {
            if (string.IsNullOrEmpty(cpno))
            {
                return BadRequest(0);
            }
            var response = _Service.GetCircularDetails(cpno);
            if (response.Count==0)
            {
                return NotFound("No item found");
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetTempDataByUser(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            var response = _Service.GetTempDataByUser(userId);

            return Ok(response);
        }
        [HttpPost]
        public IActionResult saveTempPriceChanged(Buy model)
        {
            if (model == null)
            {
                return NotFound();
            }
            var res = _Service.saveTempPriceChanged(model);
            if(res == false)
            {
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult deleteTempPriceChanged(TempPriceChanged model)
        {
            if (model == null)
            {
                return NotFound();
            }
            var res = _Service.deleteTempPriceChanged(model);
            if (res == false)
            {
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpPost]
        public IActionResult savePriceChanged(List<TempPriceChanged> tempData)
        {
            if (tempData == null)
            {
                return NotFound();
            }
            var res = _Service.savePriceChanged(tempData, out string errMsg);
            if (res == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(res);
        }
    }
}
