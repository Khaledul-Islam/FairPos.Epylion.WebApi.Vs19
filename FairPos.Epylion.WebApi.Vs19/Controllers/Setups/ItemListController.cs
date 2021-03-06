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
    public class ItemListController : ControllerBase
    {
        private readonly IItemListService _itemList;

        public ItemListController(IItemListService itemList)
        {
            _itemList = itemList;
        }

        [HttpGet]
        public IActionResult GetItemList()
        {
            var response = _itemList.SelectAll();
            return Ok(response);
        }

        [HttpGet]
        public IActionResult EditItemDetailsBySbarcode(string sbarocde)
        {
            var response = _itemList.EditItemDetailsBySbarcode(sbarocde);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult StyleSizeDetailsBySbarcode(string sbarocde)
        {
            var response = _itemList.StyleSizeDetailsBySbarcode(sbarocde);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult StyleSizeDetailsByBarcode(string barocde)
        {
            var response = _itemList.StyleSizeDetailsByBarcode(barocde);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult ApprovedItembyBarcode(string barocde)
        {
            var response = _itemList.ApprovedItembyBarcode(barocde);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GeneratesBarCode(string SupID, string PrdID)
        {
            var response = _itemList.GenerateSBarcode(SupID, PrdID);
            return Ok(response);

        }
        [HttpGet]
        public IActionResult GenerateBarCode()
        {
            var response = _itemList.GenerateBarcode();
            return Ok(response);

        }
        [HttpGet]
        public IActionResult ItemListDDL(string SupID, string PrdID)
        {
            var response = _itemList.ItemDDL(SupID,PrdID);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult ApproveItemListbySupID(string SupID)
        {
            var response = _itemList.ApproveItemListbySupID(SupID);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult DeleteStyleSize(string sBarcode)
        {
            var response = _itemList.Delete(new StyleSize { sBarcode = sBarcode });
            return Ok(response);
        }
        [HttpPost]
        public IActionResult Update([FromBody]StyleSize model)
        {
            var response = _itemList.Update(model);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Insert([FromBody]StyleSize model)
        {
            var response = _itemList.Insert(model);
            return Ok(response);
        }

        [HttpPost]
        public DataTablesResponse GetForDataTable([FromBody] DataTablesRootObject dataTablesRootObject)
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
                List<StyleSize> oListAll = _itemList.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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
        public DataTablesResponse GetForDataTableForAuth([FromBody] DataTablesRootObject dataTablesRootObject)
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
                List<StyleSize> oListAll = _itemList.GetsForDataTables(dataTablesRootObject.start, 99999, sortInformAtion, ref error, searchText);

                r.data = oListAll.Where(m=>m.isAuthorized == false).Take(200);

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
        public IActionResult UpdateAuthorized([FromBody] List<StyleSize> model)
        {
            try
            {
                foreach (StyleSize s in model)
                {
                    bool result = _itemList.UpdateAuth(s);
                }

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}
