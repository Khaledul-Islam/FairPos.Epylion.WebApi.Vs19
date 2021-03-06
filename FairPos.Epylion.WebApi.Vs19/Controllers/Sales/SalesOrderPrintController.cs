using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Service.Sales;
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
    public class SalesOrderPrintController : ControllerBase
    {
        private readonly ISalesOrderPrintService _service;

        public SalesOrderPrintController(ISalesOrderPrintService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult SalesOrderPrint(string so)
        {
            if (so == null)
            {
                return BadRequest();
            }
            var response = _service.IS_SO_PRINTED(so);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult PendingSalesOrder()
        {
            var response = _service.PendingSalesOrder();
            if (response == null || response.Count == 0)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult PendingSalesOrderStaff()
        {
            var response = _service.PendingSalesOrderStaff();
            if (response == null || response.Count == 0)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SalesOrderDeliverySave(List<SALES_ORDER> model)
        {
            if(model.Count==0 || model == null)
            {
                return BadRequest();
            }
            var response = _service.SalesOrderDeliverySave(model);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SalesOrderDeliverySaveStaff(List<SALES_ORDER> model)
        {
            if(model.Count==0 || model == null)
            {
                return BadRequest();
            }
            var response = _service.SalesOrderDeliverySaveStaff(model);
            return Ok(response);
        }
    }
}
