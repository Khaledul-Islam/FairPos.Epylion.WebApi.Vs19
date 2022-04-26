using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service.Setups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Sales
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class SalesStaffController : ControllerBase
    {
        private readonly ISalesStaffService _repo;

        public SalesStaffController(ISalesStaffService repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult SalesOrderSave(List<TempSalesOrder> model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _repo.SalesOrderSave(model, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == "false")
            {
                return BadRequest(errMsg);
            }
            return Ok(response);

        }
        [HttpPost]
        public IActionResult TempSalesOrderSave(Buy model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _repo.TempSalesOrderSave(model, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == false)
            {
                return BadRequest($"response {response} with Error Messaage : " + errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult saveTopUp(TopUp model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _repo.saveManagementStaffTopUp(model);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetProductsByProductID(string PrdId)
        {
            var response = _repo.GetProductsByOrgBarcode(PrdId, "N");
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
    }
}
