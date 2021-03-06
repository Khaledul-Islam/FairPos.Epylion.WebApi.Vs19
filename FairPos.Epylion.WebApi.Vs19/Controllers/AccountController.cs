using FairPos.Epylion.Models;
using FairPos.Epylion.Service;
using FairPos.Epylion.WebApi.Vs19.JWT;
using FairPos.Epylion.WebApi.Vs19.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    //[EnableCors("CorsApi")]
    //[DisableCors]
    public class AccountController : ControllerBase
    {
        //https://github.com/dotnet-labs/JwtAuthDemo
        private readonly ILogger<AccountController> _logger;
        private readonly IUsersWebService _userService;
        private readonly IUserCounterService _userCounterService;
        private readonly ILoginConferenceService _loginConferenceService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IUsersWebService userService, ILoginConferenceService loginConferenceService, IUserCounterService userCounterService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _userCounterService = userCounterService;
            _loginConferenceService = loginConferenceService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UsersWeb request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            request.Password = GlobalFunction.EncryptPlainTextToCipherText(request.Password);

            if (!_userService.IsValidUserCredentials(request.UserId, request.Password))
            {
                return Unauthorized();
            }

            var user = _userService.FindById(request.UserId);

            //var role = _userService.GetUserRole(request.UserId);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserId),
                new Claim(ClaimTypes.Role, GlobalFunction.SystemRole)
            };

            List<LoginConference> lstLoginConference = _loginConferenceService.SelectAll();

            if (lstLoginConference.Where(x => x.CounterId == request.CounterId && x.ShopID == user.usersShop[0].ShopID && x.UserId != request.UserId).ToList().Count > 0)
            {
                return Ok(new UsersWeb
                {
                    isActive = false,
                    AccessToken = "Another User Already Logged in this Counter"
                });
            }
            else
            {
                LoginConference aLoginConference_todelete = lstLoginConference.Where(x => x.UserId == request.UserId).FirstOrDefault();
                if (aLoginConference_todelete != null)
                {
                    _loginConferenceService.Delete(aLoginConference_todelete);
                }

                LoginConference aLoginConference = new LoginConference() { CounterId = request.CounterId, UserId = request.UserId, ShopID = user.usersShop[0].ShopID, LoginTime = DateTime.Now };
                _loginConferenceService.Insert(aLoginConference);
            }

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserId, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserId}] logged in the system.");
            return Ok(new UsersWeb
            {
                isActive = true,
                UserId = request.UserId,
                usersShop = user.usersShop,
                CounterId = request.CounterId,
                //Role = role,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [HttpGet("user")]
        [Authorize(Policy = nameof(Policy.Account))]
        public ActionResult GetCurrentUser()
        {
            return Ok(new UsersWeb
            {
                UserId = User.Identity?.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                OriginalUserName = User.FindFirst("OriginalUserName")?.Value
            });
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public ActionResult Logout([FromBody] UsersWeb request)
        {
            // var userName = User.Identity?.Name;
            //   var user = _userService.FindById(request.UserId);

            LoginConference aLoginConference = _loginConferenceService.SelectAll().Where(x => x.UserId == request.UserId).FirstOrDefault();
            if (aLoginConference != null)
            {
                _loginConferenceService.Delete(aLoginConference);
            }


            _jwtAuthManager.RemoveRefreshTokenByUserName(request.UserId);
            _logger.LogInformation($"User [{request.UserId}] logged out the system.");
            return Ok(new Result() { Status = true });
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        //   [Authorize(Policy = nameof(Policy.Account))]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                //  var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, request.accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new UsersWeb
                {
                    UserId = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }



        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("create")]
        public ActionResult Create([FromBody] UsersWeb request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                {
                    return BadRequest("user name is required");
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("user password");
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    if (!GlobalFunction.IsValidEmail(request.Email))
                    {
                        return BadRequest("invalid email address");
                    }
                }

                request.Password = GlobalFunction.EncryptPlainTextToCipherText(request.Password);

                var service = _userService.Insert(request);

                return Created("data created", new { CreateDate = DateTime.Now });

                //return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("update")]
        public ActionResult Update([FromBody] UsersWeb request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                {
                    return BadRequest("user name is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}

                if (!string.IsNullOrEmpty(request.Email))
                {
                    if (!GlobalFunction.IsValidEmail(request.Email))
                    {
                        return BadRequest("invalid email address");
                    }
                }

                if (!string.IsNullOrEmpty(request.Password))
                {
                    request.Password = GlobalFunction.EncryptPlainTextToCipherText(request.Password);
                }

                var service = _userService.Update(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpPost("delete")]
        public ActionResult Delete(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return BadRequest("user name is required");
                }

                //if (string.IsNullOrEmpty(request.Password))
                //{
                //    return BadRequest("user password");
                //}
               


                var service = _userService.Delete(new UsersWeb { UserId= userName });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet()]
        public ActionResult<List<UsersWeb>> Get()
        {
            try
            {

                var data = _userService.SelectAll();

                foreach(var d in data)
                {
                    d.Password = "";
                    d.EmailPassword = "";
                }

                return data;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }


        [Authorize(Policy = nameof(Policy.Account))]
        [HttpGet("get_details_by_user")]
        public ActionResult<Result> GetDetailsByUser(string username)
        {
            try
            {

                var data = _userService.FindById(username);

                if (data != null)
                {
                    data.Password = "";
                    data.EmailPassword = "";
                }
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
                        if(dataTablesRootObject.columns[dataTablesRootObject.order[0].column].data == "username")
                        {
                            dataTablesRootObject.columns[dataTablesRootObject.order[0].column].data = "UserId";
                        }
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
                List<UsersWeb> oListAll = _userService.GetsForDataTables(dataTablesRootObject.start, dataTablesRootObject.length, sortInformAtion, ref error, searchText);

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

        [AllowAnonymous]
        [HttpGet("select_all_active_counter")]
        public IActionResult SelectAllActiveUserCounter()
        {
            List<UserCounter> oListAll = _userCounterService.SelectAll().Where(x => x.IsActive == true).ToList();
            return Ok(oListAll);
        }


    }
}
