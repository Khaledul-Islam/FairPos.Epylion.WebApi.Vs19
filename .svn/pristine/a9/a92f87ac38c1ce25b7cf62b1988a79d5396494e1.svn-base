using FairPos.Epylion.Models;
using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service;
using FairPos.Epylion.Service.Setups;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Setups
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopListController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IShopListService _shopService;

        public ShopListController(ILogger<AccountController> logger, IShopListService shopService)
        {
            _logger = logger;
            _shopService = shopService;
        }



        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("create")]
        public ActionResult Create([FromBody] ShopList request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ShopName))
                {
                    return BadRequest("shop name is required");
                }



                var service = _shopService.Insert(request);

                return Created("data created", new { CreateDate = DateTime.Now });

                //return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("update")]
        public ActionResult Update([FromBody] ShopList request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ShopName))
                {
                    return BadRequest("shop name is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}



                var service = _shopService.Update(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("delete")]
        public ActionResult Delete(string shopid)
        {
            try
            {
                if (string.IsNullOrEmpty(shopid))
                {
                    return BadRequest("shopid is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}



                var service = _shopService.Delete(new ShopList { ShopID = shopid });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet()]
        public ActionResult<List<ShopList>> Get()
        {
            try
            {

                var data = _shopService.SelectAll();
                return data;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet("get_details_by_id")]
        public ActionResult<Result> GetDetailsById(string id)
        {
            try
            {

                var data = _shopService.FindById(id);

                Result r = new Result();
                r.Data = data;
                r.Status = true;
                return r;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [HttpPost("get_for_datatable")]
        [Authorize(Policy = nameof(Policy.Account))]
        //[Authorize]
        public DataTablesResponse GetForDataTable([FromBody] DataTablesRootObject dataTablesRootObject)
        {
            //[FromBody] DataTablesRootObject dataTablesRootObject
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
                List<ShopList> oListAll = _shopService.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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
        [HttpPost("SaveSoftwareSetting")]
        [Authorize(Policy = nameof(Policy.Account))]
        public IActionResult SaveSoftwareSetting(GlobalSetup gs)
        {
            if (gs == null)
            {
                return BadRequest();
            }
            var response = _shopService.SaveSoftwareSetting(gs);
            return Ok(response);
        }

        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet("GetSoftwareSetting")]
        public IActionResult GetSoftwareSetting(string storeID)
        {
            if (string.IsNullOrEmpty(storeID))
            {
                return BadRequest();
            }
            try
            {
                var response = _shopService.GetSoftwareSetting(storeID, out string errMsg);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    return BadRequest(errMsg);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }


        }



    }
}
