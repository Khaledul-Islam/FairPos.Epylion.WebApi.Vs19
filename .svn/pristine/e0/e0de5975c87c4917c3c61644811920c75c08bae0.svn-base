using FairPos.Epylion.Models.Operations;
using FairPos.Epylion.Service.Operations;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
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
    public class ArrivalEditController : ControllerBase
    {
        private readonly IArrivalEditService _service;

        public ArrivalEditController(IArrivalEditService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetArrivalNoQC(string UserID, string ShopID)
        {
            var response = _service.GetArrivalNoQC(UserID,ShopID);
            if (response == null)
            {
                return BadRequest("No result found");
            }
            return Ok(response);
        
        }
        [HttpGet]
        public IActionResult GetsTempArrivalUpdate()
        {
            var response = _service.GetsTempArrivalUpdate();
            if (response == null)
            {
                return BadRequest("No result found");
            }
            return Ok(response);
        
        }
        [HttpGet]
        public IActionResult GetTempArrivalUpdateByUser(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return BadRequest();
            }
            var response = _service.GetTempArrivalUpdateByUser(UserID);
            if (response == null)
            {
                return BadRequest("No result found");
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult updateTmpArrival(TempArrivalUpdate item)
        {
            if (item==null)
            {
                return BadRequest();
            }
            var response = _service.updateTmpArrival(item, out string errMsg);
            if (response == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult updateArrival(TempArrivalUpdate item)
        {
            if (item==null)
            {
                return BadRequest();
            }
            var response = _service.updateArrival(item, out string errMsg);
            if (response == "false" || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult deleteTmpArrival(TempArrivalUpdate item)
        {
            if (item==null)
            {
                return BadRequest();
            }
            var response = _service.deleteTmpArrival(item);
            if (response == false)
            {
                return BadRequest("Operation Failed");
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult LoadTempArrivalUpdate(string arrivalNo, string UserID)
        {
            if (string.IsNullOrEmpty(arrivalNo) || string.IsNullOrEmpty(UserID))
            {
                return BadRequest();
            }
            var response = _service.LoadTempArrivalUpdate(arrivalNo,UserID,out string errMsg);
            if (response == false || !string.IsNullOrEmpty(errMsg))
            {
                return BadRequest(errMsg);
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
                List<TempArrivalUpdate> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, dataTablesRootObject.search.username, searchText);

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
