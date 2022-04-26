using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service;
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
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _empService;

        public EmployeeController(IEmployeeService empService)
        {
            _empService = empService;
        }

        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost]
        public IActionResult Create([FromBody] Employee request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Employee name is required");
                }

                //request.CreateBy = User.Identity.Name;
                //request.CreateDate = DateTime.Now;

                var service = _empService.Insert(request);

                return Created("data created", new { CreateDate = DateTime.Now });

                //return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }
        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet()]
        public IActionResult Get()
        {
            try
            {

                var data = _empService.SelectAll();
                if (data == null || data.Count == 0)
                {
                    return BadRequest(data);
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost]
        public IActionResult Update([FromBody] Employee request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return BadRequest("Employee name is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}

                //request.UpdateBy = User.Identity.Name;
                request.UpdateDate = DateTime.Now;

                var service = _empService.Update(request);

                return Ok(service);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }

        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Employee id is required");
                }

                decimal EID;
                decimal.TryParse(id, out EID);

                var service = _empService.Delete(new Employee { EmpID = EID }); ;

                return Ok(service);
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

                var data = _empService.FindById(id);

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
                List<Employee> oListAll = _empService.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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

        [HttpPost("Employee_Sync")]
        // [Authorize(Policy = nameof(Policy.Account))]
        public ActionResult Sync()
        {
            try
            {

                bool service = _empService.Sync();

                //return Ok("Sync Done " + service);
               return Ok( new { CreateDate = DateTime.Now });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }
    }
}
