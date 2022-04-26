using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Service.Operations;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Operations
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = nameof(Policy.Account))]
    public class ArrivalCheckController : ControllerBase
    {
        private readonly IApprivalCheckService _service;

        public ArrivalCheckController(IApprivalCheckService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetACSupplierDDL(string UserID, string ShopID)
        {
            var response = _service.GetACSupplierDDL(UserID,ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetPendingPONo(string SupID, string ShopID)
        {
            var response = _service.GetPendingPONo(SupID, ShopID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempArrival(string UserID)
        {
            var response = _service.GetTempArrival(UserID);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetTempArrivalByChallanNo(string chln)
        {
            var response = _service.GetTempArrivalByChallanNo(chln);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetChallanDetails(string POno)
        {
            var response = _service.GetChallanDetails(POno);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveTempArrival(List<BuyOrder> model)
        {
            var response = _service.SaveTempArrival(model);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SaveArrival(List<TempArrival> model)
        {
            if(model==null || model.Count == 0)
            {
                return BadRequest();
            }
            var response = _service.SaveArrival(model);
            if (response == "false")
            {
                return BadRequest("Error Occured when save arrival. Please see log for more details");
            }
            return Ok(response);
        }
        [HttpDelete]
        public IActionResult RemoveTempArrival(string UserID)
        {
            var response = _service.RemoveTempArrival(UserID);
            if (response == false)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult saveTempArrivalSingleItem(TempArrival model)
        {
            var response = _service.SaveTempArrivalItem(model);
            if (response == false)
            {
                return BadRequest();
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
                List<TempArrival> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, searchText);

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
