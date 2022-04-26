using FairPos.Epylion.Models.Setups;
using FairPos.Epylion.Service;
using FairPos.Epylion.Service.Setups;
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
    public class MemberCategoryProductController : ControllerBase
    {
        private readonly IMemberCategoryProductService _service;

        public MemberCategoryProductController(IMemberCategoryProductService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult Update([FromBody] List<MemberCategoryProduct> request)
        {
            var checkedItem = request.Where(a => a.Check == true).ToList();

            var CatPrdId = request.FirstOrDefault().CatPrdId;
            var MemCatId = request.FirstOrDefault().MemCatId;
            List<MemberCategoryProduct> oList = new();

            foreach (var item in checkedItem)
            {
                MemberCategoryProduct ei = new MemberCategoryProduct();
                ei.LimitQty = item.LimitQty;
                ei.PrdID = item.PrdID;
                ei.CatPrdId = item.CatPrdId;
                ei.MemCatId = MemCatId;
                oList.Add(ei);
            }

            if (oList.Count == 0)
            {
                return BadRequest("No Record Found");
            }
            _service.Delete(request.FirstOrDefault());

            foreach (var d in oList)
            {
                _service.Insert(d);
            }
            _service.UpdateEmployeeWiseProductLimit(MemCatId.ToString());
            return Ok();
        }
        [HttpGet]
        public IActionResult FindAll()
        {
            var response = _service.FindAll();
            if(response==null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult FindById(string CatPrdID)
        {
            var response = _service.FindById(CatPrdID);
            if(response==null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult FindByMemCatId(string MemCatId)
        {
            var response = _service.FindByMemCatId(MemCatId);
            if(response==null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
    }
}
