using FairPos.Epylion.Models;
using FairPos.Epylion.Service;
using FairPos.Epylion.Service.Setups;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers.Setups
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {


        private readonly ISupplierService _shopService;

        public SupplierController(ISupplierService shopService)
        {
            _shopService = shopService;
        }



        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("create")]
        public ActionResult Create() //[FromBody] Supplier request
        {
            try
            {
                var DocumentMain = JsonConvert.DeserializeObject<Supplier>(Request.Form["DocumentMain"].ToString());
                var DocumentDoc = JsonConvert.DeserializeObject<SupplierDoc>(Request.Form["DocumentDoc"].ToString());

                if(Request.Form.Files.Count > 0)
                {
                    foreach(var d in Request.Form.Files)
                    {
                        IFormFile file = d;
                        switch (file.Name)
                        {
                            case "Document1":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.TradeFile = ms.ToArray();
                                    DocumentDoc.TradeFileType = file.FileName;
                                }
                                break;
                            case "Document2":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.BSTIFile = ms.ToArray();
                                    DocumentDoc.BSTIFileType = file.FileName;
                                }
                                break;
                            case "Document3":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.VatFile = ms.ToArray();
                                    DocumentDoc.VatFileType = file.FileName;
                                }
                                break;
                            case "Document4":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.TinFile = ms.ToArray();
                                    DocumentDoc.TinFileType = file.FileName;
                                }
                                break;
                            case "Document5":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.OtherDoc = ms.ToArray();
                                    DocumentDoc.OtherDocType = file.FileName;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(DocumentMain.Supname))
                {
                    return BadRequest("Supplier name is required");
                }
                if (string.IsNullOrEmpty(DocumentMain.RegName))
                {
                    return BadRequest("Registration name is required");
                }

                //request.CreateBy = User.Identity.Name;
                //request.CreateDate = DateTime.Now;

                DocumentMain.supplierDoc = DocumentDoc;

                var service = _shopService.Insert(DocumentMain);

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
        public ActionResult Update() //[FromBody] Supplier request
        {

            try
            {
                var DocumentMain = JsonConvert.DeserializeObject<Supplier>(Request.Form["DocumentMain"].ToString());
                var DocumentDoc = JsonConvert.DeserializeObject<SupplierDoc>(Request.Form["DocumentDoc"].ToString());

                if (Request.Form.Files.Count > 0)
                {
                    foreach (var d in Request.Form.Files)
                    {
                        IFormFile file = d;
                        switch (file.Name)
                        {
                            case "Document1":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.TradeFile = ms.ToArray();
                                    DocumentDoc.TradeFileType = file.FileName;
                                }
                                break;
                            case "Document2":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.BSTIFile = ms.ToArray();
                                    DocumentDoc.BSTIFileType = file.FileName;
                                }
                                break;
                            case "Document3":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.VatFile = ms.ToArray();
                                    DocumentDoc.VatFileType = file.FileName;
                                }
                                break;
                            case "Document4":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.TinFile = ms.ToArray();
                                    DocumentDoc.TinFileType = file.FileName;
                                }
                                break;
                            case "Document5":
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    DocumentDoc.OtherDoc = ms.ToArray();
                                    DocumentDoc.OtherDocType = file.FileName;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(DocumentMain.Supname))
                {
                    return BadRequest("Supplier name is required");
                }
                if (string.IsNullOrEmpty(DocumentMain.RegName))
                {
                    return BadRequest("Registration name is required");
                }

                //request.CreateBy = User.Identity.Name;
                //request.CreateDate = DateTime.Now;

                DocumentMain.supplierDoc = DocumentDoc;
                if(DocumentMain.Fired.HasValue && DocumentMain.Fired.Value == true)
                {
                    DocumentMain.DOE = DateTime.Now;
                }

                var service = _shopService.Update(DocumentMain);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("delete")]
        public ActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Supplier id is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}



                var service = _shopService.Delete(new Supplier { SupID = id }); ;

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet()]
        public ActionResult<List<Supplier>> Get()
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


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet("get_doc_details_by_id")]
        public ActionResult<Result> GetDocDetailsById(string id)
        {
            try
            {

                var data = _shopService.FindDocumentById(id);
                

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

        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet("get_doc_details_by_id2")]
        public ActionResult GetDocDetailsById2(string id, string fileType = "")
        {
            try
            {

                var data = _shopService.FindDocumentById(id);
                if (string.IsNullOrEmpty(fileType))
                {
                    data.BSTIFile = null;
                    data.TradeFile = null;
                    data.VatFile = null;
                    data.TinFile = null;
                    data.OtherDoc = null;
                }
                else if ((fileType) == "BSTIFileType")
                {
                    string fileType_ = "application/octet-stream";
                    if (data.BSTIFileType.ToLower().Contains("pdf"))
                    {
                        fileType_ = "application/pdf";
                    }
                    return File(data.BSTIFile, fileType_ , data.BSTIFileType);
                }
                else if ((fileType) == "TradeFileType")
                {
                    string fileType_ = "application/octet-stream";
                    if (data.TradeFileType.ToLower().Contains("pdf"))
                    {
                        fileType_ = "application/pdf";
                    }
                    return File(data.TradeFile, fileType_, data.TradeFileType);
                }
                else if ((fileType) == "VatFileType")
                {
                    string fileType_ = "application/octet-stream";
                    if (data.VatFileType.ToLower().Contains("pdf"))
                    {
                        fileType_ = "application/pdf";
                    }
                    return File(data.VatFile, fileType_, data.VatFileType);
                }
                else if ((fileType) == "TinFileType")
                {
                    string fileType_ = "application/octet-stream";
                    if (data.TinFileType.ToLower().Contains("pdf"))
                    {
                        fileType_ = "application/pdf";
                    }
                    return File(data.TinFile, fileType_, data.TinFileType);
                }
                else if ((fileType) == "OtherDocType")
                {
                    string fileType_ = "application/octet-stream";
                    if (data.OtherDocType.ToLower().Contains("pdf"))
                    {
                        fileType_ = "application/pdf";
                    }
                    return File(data.OtherDoc, fileType_, data.OtherDocType);
                }

                Result r = new Result();
                r.Data = data;
                r.Status = true;
                return NotFound();
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
                List<Supplier> oListAll = _shopService.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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
