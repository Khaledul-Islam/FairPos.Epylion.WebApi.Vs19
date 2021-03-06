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
    [ApiController]
    [Authorize(Policy = nameof(Policy.Account))]
    public class ItemConversionController : ControllerBase
    {
        private readonly IItemConversionService _service;

        public ItemConversionController(IItemConversionService service)
        {
            _service = service;
        }

        [HttpDelete]
        public IActionResult DeleteTempConversionStock(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return NotFound();
            }
            var response = _service.DeleteTempConversionStock(UserID);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetByBarcodeExp(string expbarcode)
        {
            if (string.IsNullOrEmpty(expbarcode))
            {
                return NotFound();
            }
            var response = _service.GetByBarcodeExp(expbarcode);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetItemDetails(string ParentSbarcode, DateTime parentItemExpire)
        {

            var response = _service.GetItemDetails(ParentSbarcode, parentItemExpire);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetProductsSupplier(string SupId, string ShopID)
        {
            if (string.IsNullOrEmpty(SupId))
            {
                return NotFound();
            }
            var response = _service.GetProductsSupplier(SupId, ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetNewBarcodeBySbarcode(string sBarcode, decimal qty)
        {
            var response = _service.GetNewBarcodeBySbarcode(sBarcode, qty);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult TempConcStockList()
        {
            var response = _service.TempConvStockList();
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult TempConvStockListByID(string UserID)
        {
            var response = _service.TempConvStockListByID(UserID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveTempConvStock([FromBody] TempConversionStock model)
        {
            var response = _service.SaveTempConvStock(model);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveConvStock([FromBody]List<StyleSizeCoversition> model)
        {
            var response = _service.SaveConvStock(model, out string errMsg);
            if (response == "false" || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
    }
}
