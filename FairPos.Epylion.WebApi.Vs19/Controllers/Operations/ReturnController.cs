using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using FairPos.Epyllion.Repository.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Operations
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = nameof(Policy.Account))]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnRepository _service;

        public ReturnController(IReturnRepository service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetReturnReasonDDL()
        {
            var response = _service.GetReturnReasonDDL();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempDataByUser(string userId, string shopId)
        {
            var response = _service.GetTempDataByUser(userId, shopId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempDataByBarcode(string barcode)
        {
            var response = _service.GetTempDataByBarcode(barcode);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult TempReturnSave([FromBody] Buy model)
        {
            var response = _service.TempReturnSave(model);
            if (response == false)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult TempReturnDelete([FromBody] TempStockReturnShop model)
        {
            var response = _service.TempReturnDelete(model);
            if (response == false)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult StockReturnShopSave(List<TempStockReturnShop> model)
        {
            var response = _service.StockReturnShopSave(model);
            if (response == "false")
            {
                return NotFound();
            }
           
            return Ok(response);
        }
        [HttpPost]
        public DataTablesResponse GetTempDataTable([FromBody] DataTablesRootObject dataTablesRootObject)
        {
            DataTablesResponse r = new();
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
                List<TempStockReturnShop> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, dataTablesRootObject.search.branchid, searchText);

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
