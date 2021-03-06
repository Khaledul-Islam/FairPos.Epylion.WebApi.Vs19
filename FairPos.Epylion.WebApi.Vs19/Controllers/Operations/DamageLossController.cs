using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service.Operations;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
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
    public class DamageLossController : ControllerBase
    {
        private readonly IDamageLossService _service;

        public DamageLossController(IDamageLossService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetMainProductsSupplier(string SupId, string ShopID)
        {
            var response = _service.GetMainProductsSupplier(SupId, ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetDamageProductsSupplier(string SupId)
        {
            var response = _service.GetDamageProductsSupplier(SupId);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetDamageBarcodeBySbarcode(string sBarcode, decimal qty)
        {
            var response = _service.GetDamageBarcodeBySbarcode(sBarcode, qty);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetDamageBarcodeExp(string expbarcode)
        {
            var response = _service.GetDamageBarcodeExp(expbarcode);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetStockDMLTemp(string BarCode)
        {
            var response = _service.GetStockDMLTemp(BarCode);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempDataByUser(string UserID, string ShopID)
        {
            var response = _service.GetTempDataByUser(UserID, ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult StockDMLTempSave([FromBody]Buy model)
        {
            var response = _service.StockDMLTempSave(model);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult StockDMLSave(List<StockDMLTemp> model)
        {
            var response = _service.StockDMLSave(model, out string errMsg);
            if (response == "false" || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult StockDMLTempDelete([FromBody]StockDMLTemp model)
        {
            var response = _service.StockDMLTempDelete(model);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public DataTablesResponse GetTempDataTable([FromBody] DataTablesRootObject dataTablesRootObject)
        {
            DataTablesResponse r = new ();
            try
            {
                string searchText = "";
                if (dataTablesRootObject.search != null)
                {
                    searchText = dataTablesRootObject.search.value;
                }


                #region single sort code
                string sortInformAtion = "";

                if (dataTablesRootObject.order != null && dataTablesRootObject.order.Count > 0)
                {
                    if (dataTablesRootObject.columns != null && dataTablesRootObject.columns.Count > 0)
                    {
                        sortInformAtion = "ORDER BY " + dataTablesRootObject.columns[dataTablesRootObject.order[0].column].data + " " + dataTablesRootObject.order[0].dir;
                    }
                    //dataTablesRootObject.order[0].column
                }
                if (string.IsNullOrEmpty(sortInformAtion))
                {
                    sortInformAtion = "ORDER BY " + dataTablesRootObject.columns[0].data + " asc";
                }
                #endregion

                string error = "";
                List<StockDMLTemp> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, dataTablesRootObject.search.branchid, searchText);

                r.data = oListAll;

                r.draw = dataTablesRootObject.draw;
                r.error = error;
                if (oListAll != null && oListAll.Count > 0)
                {
                    r.recordsTotal = oListAll[0].RecordCount;
                    r.recordsFiltered = oListAll[0].RecordFilter;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.StackTrace);
            }
            return r;

        }

    }
}
