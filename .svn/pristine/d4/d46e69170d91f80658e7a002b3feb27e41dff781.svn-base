using FairPos.Epylion.Models;
using FairPos.Epylion.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {

        IMenuService serviceMenu;

        public MenusController(IMenuService _serviceMenu)
        {
            serviceMenu = _serviceMenu;
        }

        //ICompanyService serviceCompany = new CompanyService(GlobalClass.GetDbConnection);

        [Authorize]
        [HttpGet("select_all_parent_by_user")]
        public ActionResult<Result> SelectAllParentByUser(string userName)
        {
            Result r = new Result();
            try
            {
                List<WebMenu> oListAll = null;
                if (userName == GlobalFunction.SystemUser)
                {
                    oListAll = serviceMenu.SelectAllParent();
                }
                else
                {
                    oListAll = serviceMenu.SelectAllParentByUser(userName);
                }

                

                r.Data = oListAll;
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }


        [Authorize]
        [HttpGet("select_all_child_by_user")]
        public ActionResult<Result> SelectAllChildByUser(string userName)
        {
            Result r = new Result();
            try
            {
                List<WebMenu> oListAll = null;
                if (userName == GlobalFunction.SystemUser)
                {
                    oListAll = serviceMenu.SelectAllChild();
                }
                else
                {
                    oListAll = serviceMenu.SelectAllChildByUser(userName);
                }


                r.Data = oListAll;
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }


        [HttpGet("select_all_parent")]
        [Authorize(Policy = nameof(Policy.Account))]
        public ActionResult<Result> SelectAllParent()
        {
            Result r = new Result();
            try
            {
                List<WebMenu> oListAll = null;

                oListAll = serviceMenu.SelectAllParent();


                r.Data = oListAll;
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }

        [Authorize]
        [HttpGet("select_all_child")]
        public Result SelectAllChild()
        {
            Result r = new Result();
            try
            {
                List<WebMenu> oListAll = null;

                oListAll = serviceMenu.SelectAllChild();

                r.Data = oListAll;
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }


        [Authorize]
        [HttpGet("select_all_by_user")]
        public Result SelectAllByUser(string userName)
        {
            Result r = new Result();
            try
            {
                List<WebMenu> oListAll = null;
                if (userName == GlobalFunction.SystemUser)
                {
                    oListAll = serviceMenu.SelectAll("");
                }
                else
                {
                    oListAll = serviceMenu.SelectByUserID(userName);
                }


                r.Data = oListAll;
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }


        [Authorize]
        [HttpPost("[action]")]
        public Result Menus_InsertUpdate([FromBody] List<UsersPermission> model)
        {
            Result r = new Result();
            r.Status = true;
            try
            {

                if (model.Count == 0)
                {
                    r.Status = false;
                    r.Message = "no data for save";
                }

                if (string.IsNullOrEmpty(model[0].UserId))
                {
                    r.Status = false;
                    r.Message = "Enter user name";
                }



                if (r.Status == false)
                    return r;



                r.Status = true;
                string msg = "";



                ///model.BANK_ID = StaticData.GetMaxId(GlobalClass.GetDbConnection, "BANK_ID", "4", "0001", "BankList", "BNK").ToString();
                r.Status = serviceMenu.AddUpdate(model, ref msg);
                r.Message = msg;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message + ex.StackTrace;
            }
            return r;

        }



    }
}
