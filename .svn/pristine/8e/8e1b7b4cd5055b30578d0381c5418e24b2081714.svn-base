using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Models.Transfer;
using FairPos.Epylion.Service.Transfer;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Transfer
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ShopToShopController : ControllerBase
    {
        private readonly IShopToShopService _service;

        public ShopToShopController(IShopToShopService service)
        {
            _service = service;
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
    }
}
