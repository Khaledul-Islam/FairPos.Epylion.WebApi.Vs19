using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service.Setups;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Setups
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class MemberCategoryController : ControllerBase
    {
        private readonly IMemberCategoryService _service;

        public MemberCategoryController(IMemberCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {

                var data = _service.SelectAll();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [HttpGet]
        public IActionResult GetDetailsById(string id)
        {
            try
            {

                var data = _service.FindById(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] MemberCategory request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.MemCatName))
                {
                    return BadRequest("Member Category Name is required");
                }

                var service = _service.Insert(request);

                return Created("data created", new { CreateDate = DateTime.Now });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult Update([FromBody] MemberCategory request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.MemCatName))
                {
                    return BadRequest("Member Category Name is required");
                }


                var service = _service.Update(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult Delete(string Name)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return BadRequest("Unit of measure name is required");
                }

                var service = _service.Delete(new MemberCategory { MemCatId = int.Parse(Name) });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }
        [HttpPost]
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
                List<MemberCategory> oListAll = _service.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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
