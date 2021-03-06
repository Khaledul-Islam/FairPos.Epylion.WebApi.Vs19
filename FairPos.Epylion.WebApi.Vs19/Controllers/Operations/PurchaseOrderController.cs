using FairPos.Epylion.Models.Operations;
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
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _poService;

        public PurchaseOrderController(IPurchaseOrderService poService)
        {
            _poService = poService;
        }
        #region Purchase Order
        [HttpPost]
        public IActionResult RemoveBuyOrderTemp([FromBody] BuyOrderTemp model)
        {
            if (model == null)
            {
                return NotFound();
            }
            bool response = _poService.RemoveBuyOrderTemp(model);

            return Ok();
        }
        [HttpPost]
        public IActionResult RemoveBuyOrderTempByID(string barCode)
        {
            if (barCode == null)
            {
                return NotFound();
            }
            bool response = _poService.RemoveBuyOrderTempByID(barCode);
            if (!response)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateBuyOrderTemp([FromBody] BuyOrderTemp model)
        {
            if (model == null)
            {
                return NotFound();
            }
            bool response = _poService.Insert(model);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult PurchaseOrder([FromBody] List<BuyOrderTemp> model)
        {
            if (model == null)
            {
                return NotFound();
            }
            string response = _poService.PurchaseOrder(model);
            if (response == "false")
            {
                return BadRequest("P/O Save error. See Exception trace in log");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult SupplierbyID(string SupID)
        {
            var response = _poService.SupplierbyID(SupID);
            if (response == null)
            {
                return BadRequest("response null");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetActualStockBySbarocde(string sBarcode)
        {
            var response = _poService.GetActualStockBySbarocde(sBarcode);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetBuyOrderTemps(string UserID)
        {
            var response = _poService.GetBuyOrderTemps(UserID);
            if (response == null)
            {
                return BadRequest("response null");
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetSupplierDDL()
        {
            var response = _poService.SupplierListDDL();
            if (response == null)
            {
                return BadRequest("response null");
            }
            return Ok(response);
        }

        [HttpPost]
        public DataTablesResponse GetTempDataTable([FromBody] DataTablesRootObject dataTablesRootObject)
        {
            DataTablesResponse r = new DataTablesResponse();
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
                List<BuyOrderTemp> oListAll = _poService.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, searchText);

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

        #endregion

        #region PO Edit
        [HttpGet]
        public IActionResult GetPONotReceived(string supplierId)
        {
            if (string.IsNullOrEmpty(supplierId))
            {
                return BadRequest();
            }
            var res = _poService.GetPONotReceived(supplierId);
            if (res == null || res.Count == 0)
            {
                return BadRequest("No item Found");
            }
            return Ok(res);
        }
        [HttpGet]
        public IActionResult POEditTempSave(string pono, string userid, string shopid)
        {
            if (pono == null || userid == null || shopid == null)
            {
                return BadRequest();
            }
            var res = _poService.POSaveTemp(pono, userid, shopid, out string errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(res);
        }
        [HttpGet]
        public IActionResult DeletePOEditAll(string userid, string shopid, string chln)
        {
            if (chln == null || userid == null || shopid == null)
            {
                return BadRequest();
            }
            var res = _poService.DeletePOEditAll(userid, shopid, chln, out string errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(res);
        }
        [HttpPost]
        public DataTablesResponse GetTempDataTableEdit([FromBody] DataTablesRootObject dataTablesRootObject)
        {
            DataTablesResponse r = new DataTablesResponse();
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
                List<BuyOrderTempEdit> oListAll = _poService.GetsForDataTablesPOEdit(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, dataTablesRootObject.search.branchid, searchText);

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

        [HttpPost]
        public IActionResult PurchaseOrderEditSave(List<BuyOrderTemp> model)
        {
            if (model == null)
            {
                return NotFound();
            }
            string response = _poService.PurchaseOrderEditSave(model, out string errMsg);

            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }

        [HttpPost]
        public IActionResult RemoveBuyOrderTempEditByID(string barCode)
        {
            if (barCode == null)
            {
                return NotFound();
            }
            bool response = _poService.RemoveBuyOrderTempEditByID(barCode);
            if (!response)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult updateTmpPurchaseEdit(BuyOrderTempEdit item)
        {
            if (item == null)
            {
                return NotFound();
            }
            bool response = _poService.updateTmpPurchaseEdit(item, out string errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult CreateBuyOrderEditTemp([FromBody] BuyOrderTemp model)
        {
            if (model == null)
            {
                return NotFound();
            }
            bool response = _poService.InsertPOEdit(model);
            return Ok(response);
        }

        #endregion

    }
}
