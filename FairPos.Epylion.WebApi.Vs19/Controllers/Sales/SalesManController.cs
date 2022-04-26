using FairPos.Epylion.Models.Sales;
using FairPos.Epylion.Service.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Sales
{
    [Route("[controller]/[action]")]
    [Authorize(Policy = nameof(Policy.Account))]
    [ApiController]
    public class SalesManController : ControllerBase
    {
        private readonly ISalesManService _service;

        public SalesManController(ISalesManService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult RemoveAllSalesMan(SalesMan model)
        {
            var response = _service.RemoveAllSalesMan(model);
          
            if (response != true)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetOrderDetails(string Id)
        {
            var response = _service.GetById(Id);

            if (response == null)
            {
                return BadRequest("response null");
            }

            return Ok(response);
        }

      
    }
}
