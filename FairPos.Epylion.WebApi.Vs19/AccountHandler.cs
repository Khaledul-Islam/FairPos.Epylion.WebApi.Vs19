using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19
{
    public enum Policy { Account };

    public class AccountRequirement : IAuthorizationRequirement { }


    public class AccountHandler : AuthorizationHandler<AccountRequirement>
    {
        //protected override async Task HandleRequirementAsync(
        //    AuthorizationHandlerContext context,
        //    AccountRequirement requirement)
        //{
        //    // Your logic here... or anything else you need to do.
        //    if (context.User.IsInRole("fooBar"))
        //    {
        //        context.Succeed(requirement);
        //        return;
        //    }
        //}

        //private readonly IHttpContextAccessor _accessor;

        //public AccountHandler( IHttpContextAccessor accessor)
        //{
        //    _accessor = accessor;
        //}

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   AccountRequirement requirement)
        {
            //if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
            //                                c.Issuer == "http://contoso.com"))
            //{
            //    //TODO: Use the following if targeting a version of
            //    //.NET Framework older than 4.6:
            //    //      return Task.FromResult(0);
            //    return Task.CompletedTask;
            //}

            //var dateOfBirth = Convert.ToDateTime(
            //    context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth &&
            //                                c.Issuer == "http://contoso.com").Value);

            //int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            //if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            //{
            //    calculatedAge--;
            //}

            //if (calculatedAge >= requirement.MinimumAge)
            //{



            if (context.User == null || context.User.Identity == null || context.User.Identity.IsAuthenticated == false)
            {
                return Task.CompletedTask;
            }


            //todo
            //MenuRepository menuRepository = new MenuRepository(GlobalClass.GetDbConnection);
            //UserRepository userRepository = new UserRepository(GlobalClass.GetDbConnection);
            //CompanyRepositry companyRepository = new CompanyRepositry(GlobalClass.GetDbConnection);

            //var company = companyRepository.Get();
            //if (company.UnderMaintainance)
            //{
            //    var filterContext = context.Resource as AuthorizationFilterContext;
            //    var Response = filterContext.HttpContext.Response;
            //    var message = Encoding.UTF8.GetBytes("Under Maintenance");
            //    Response.OnStarting(async () =>
            //    {
            //        filterContext.HttpContext.Response.StatusCode = 430;
            //        await Response.Body.WriteAsync(message, 0, message.Length);
            //    });
            //}

            //var authFilterCtx = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)context.Resource;
            //string authHeader = authFilterCtx.HttpContext.Request.Headers["Authorization"];
            //if (authHeader != null && authHeader.Contains("Bearer"))
            //{
            //    var token = authHeader.Replace("Bearer", "").Trim();

            //    DataModel.UserLoginConference userLoginConference = userRepository.GetLoginConfenrence(context.User.Identity.Name);
            //    //userLoginConference = null;
            //    if (userLoginConference == null || userLoginConference.TokenInfo.CompareTo(token) != 0)
            //    {
            //        var filterContext = context.Resource as AuthorizationFilterContext;
            //        var Response = filterContext.HttpContext.Response;
            //        var message = Encoding.UTF8.GetBytes("Invalid token");
            //        Response.OnStarting(async () =>
            //        {
            //            filterContext.HttpContext.Response.StatusCode = 429;
            //            await Response.Body.WriteAsync(message, 0, message.Length);
            //        });

            //    }
            //}

            if (context.User.Identity.Name == GlobalFunction.SystemUser)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //todo
            //User model = userRepository.GetByName(context.User.Identity.Name);
            //if (model == null)
            //{
            //    return Task.CompletedTask;
            //}
            //List<WMenu> models = menuRepository.SelectByUserID(model.USER_NAME);

            //if (models == null)
            //{
            //    return Task.CompletedTask;
            //}

            //AuthorizationFilterContext authorizationFilterContext =
            //context.Resource as AuthorizationFilterContext;

            //var descriptor = authorizationFilterContext?.ActionDescriptor as ControllerActionDescriptor;
            //if (descriptor != null)
            //{
            //    var actionName = descriptor.ActionName;
            //    var ctrlName = descriptor.ControllerName;

            //    foreach (var d in models)
            //    {

            //        if (d != null && (d.DefaultActionName.ToUpper() == actionName.ToUpper()))
            //        {
            //            context.Succeed(requirement);
            //            return Task.CompletedTask;
            //        }
            //    }
            //}

            context.Succeed(requirement); // todo mustbe off when above logic implemented
            return Task.CompletedTask;
        }


    }
}
