using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Service.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Operations
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetByBarcodeExpForInventory(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return BadRequest("empty barcode");
            }
            var response = _service.GetByBarcodeExpForInventory(barcode);
            if (response == null)
            {
                return NotFound("No data found");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetsTempInventory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            var response = _service.GetsTempInventory(name);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetAllTempInventory()
        {
            var response = _service.GetsTempInventory(null);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult RemoveTempInventory(TempInventory model)
        {
            if (model==null)
            {
                return BadRequest();
            }
            var response = _service.RemoveTempInventory(model);
            if (response == false)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveInventory(List<TempInventory> olist)
        {
            if (olist == null)
            {
                return BadRequest();
            }
            var response = _service.SaveInventory(olist, out string errMsg);
            if (response == "false" || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveTempInventory(TempInventory model)
        {
            if (model==null)
            {
                return BadRequest();
            }
            var response = _service.SaveTempInventory(model, out string errMsg);
            if (response == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
    }
}
