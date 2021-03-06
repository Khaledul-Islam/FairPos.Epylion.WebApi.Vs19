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
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _service;

        public SalesOrderController(ISalesOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetPendingPrintItems(string stockType)
        {
            var response = _service.GetPendingPrintItems(stockType);

            if (response.Count == 0)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetOrderDetails(string so_no)
        {
            var response = _service.GetOrderDetails(so_no);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);

        }
        [HttpGet]
        public IActionResult GetOrderDetailsByBarcode(string so_no,string sBarcode)
        {
            var response = _service.GetOrderDetails(so_no).Find(m => m.sBarcode == sBarcode);
            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetProductInfoByBarcodeBuyStaff(string barcode, string supplierId)
        {
            var response = _service.GetProductInfoByBarcodeBuyStaff(barcode, supplierId);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }


        [HttpGet]
        public IActionResult GetProductInfoByBarcodeBuyWorker(string barcode, string supplierId)
        {
            var response = _service.GetProductInfoByBarcodeBuyWorker(barcode, supplierId);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetProductInfoByBarcodeBuyMain(string barcode, string supplierId)
        {
            var response = _service.GetProductInfoByBarcodeBuyMain(barcode, supplierId);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }


        [HttpGet]
        public IActionResult GetPendingItemsByEmp(string empId, bool isAccApproved)
        {
            var response = _service.GetPendingItemsByEmp(empId, isAccApproved);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }
    }
}
