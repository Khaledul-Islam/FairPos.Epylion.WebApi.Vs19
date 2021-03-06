using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FairPos.Epylion.Service.Transfer;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Transfer
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class MainToStaffWorkerController : ControllerBase
    {
        private readonly IMainToStaffWorkerService _service;

        public MainToStaffWorkerController(IMainToStaffWorkerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTransferProducts(string SupId, string ShopID)
        {
            if (string.IsNullOrEmpty(SupId))
            {
                return NotFound();
            }
            var response = _service.GetTransferProducts(SupId, ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTransferBarcodeExp(string expbarcode)
        {
            if (string.IsNullOrEmpty(expbarcode))
            {
                return NotFound();
            }
            var response = _service.GetTransferBarcodeExp(expbarcode);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetTransferNewBarcode(string sBarcode, decimal qty)
        {
            var response = _service.GetTransferNewBarcode(sBarcode, qty);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempStock(string Barcode, string stockType)
        {
            var response = _service.GetTempStock(Barcode, stockType);
            return Ok(response);

        }
        [HttpGet]
        public IActionResult GetTempStocks()
        {
            var response = _service.GetTempStocks();
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveTempStockTransfer(Buy model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var resposne = _service.SaveTempStockTransfer(model);
            if (resposne == false)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult DeleteTempStockTransfer([FromBody]  TempStockTransfer model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _service.DeleteTempStockTransfer(model);
            if (response == false)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult SaveStockTransfer([FromBody] List<TempStockTransfer> model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = _service.SaveStockTransfer(model);
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
                List<TempStockTransfer> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, dataTablesRootObject.search.branchid, dataTablesRootObject.search.vendorno, searchText);

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
