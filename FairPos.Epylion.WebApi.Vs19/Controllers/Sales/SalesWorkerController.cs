﻿using FairPos.Epylion.Models.Common;
using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Models.Setups;
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
    public class SalesWorkerController : ControllerBase
    {
        private readonly ISalesWorkerService _service;

        public SalesWorkerController(ISalesWorkerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TempSalesOrderList(string UserId, string CounterId, string stocktype)
        {
            var response = _service.TempSalesOrderList(UserId, CounterId, stocktype);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult TempSalesOrder(string UserId, string CounterId, string stocktype)
        {
            var response = _service.TempSalesOrder(UserId, CounterId, stocktype);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetEmployeeById(string empId)
        {
            if (string.IsNullOrEmpty(empId))
            {
                return BadRequest();
            }
            var response = _service.GetEmployeeById(empId);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetInvoiceByEmployee(string empId)
        {
            var response = _service.GetInvoiceByEmployee(empId);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetProductsByProductID(string PrdId)
        {
            var response = _service.GetProductsByOrgBarcode(PrdId, "N");
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetEmployeeByRFId(string rfid)
        {
            var response = _service.GetEmployeeByRFId(rfid);
            if (response == null)
            {
                return BadRequest();
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
            var response = _service.TempSalesOrderSave(model, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == false)
            {
                return BadRequest($"response {response} with Error Messaage : " + errMsg);
            }
            return Ok(response);
        }
        
        [HttpPost]
        public IActionResult SalesOrderSave(List<TempSalesOrder> model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _service.SalesOrderSave(model, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == "false")
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult TempSalesOrderDelete(TempSalesOrder temp)
        {
            if (temp == null)
            {
                return BadRequest("Invalid Request");
            }
            var response = _service.TempSalesOrderDelete(temp, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg) || response == false)
            {
                return BadRequest($"response {response} with Error Messaage : " + errMsg);
            }
            return Ok(response);
        }
    }
}
