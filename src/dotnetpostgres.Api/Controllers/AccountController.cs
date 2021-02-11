using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.ViewModels.User.Response;
using dotnetpostgres.Api.ViewModels.User.Request;
using dotnetpostgres.Dto;
using dotnetpostgres.Response;
using dotnetpostgres.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetpostgres.Api.Controllers
{
    [Route("account"), ApiController, Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("reset")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Reset([FromBody]ResetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = await ResetAccount(model.EmailOrUsername);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = await ResetUserPassword(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }


        [HttpPost("password")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Password([FromBody]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = await ChangePassword(model.Password);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(ApiResponse<TokenViewModel>), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Token([FromBody]GetTokenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = await GetToken(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = await RegisterUser(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<UserViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var resp = await GetUser();

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        [HttpPost("settings")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public IActionResult Settings([FromBody]SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrorResponse(ModelState));
            }

            var resp = UpdateSettings(model);

            if (resp.Type != ResponseType.Success)
            {
                return BadRequest(resp);
            }

            return Ok(resp);
        }

        private async Task<ApiResponse<TokenViewModel>> GetToken(GetTokenViewModel model)
        {
            var apiResp = new ApiResponse<TokenViewModel>
            {
                Type = ResponseType.Fail
            };

            var applicationUser = new ApplicationUser
            {
                Email = model.EmailOrUsername,
                UserName = model.EmailOrUsername
            };

            var securityResp = await _accountService.GetToken(applicationUser, model.Password);

            if (securityResp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = securityResp.ErrorCode;
                return apiResp;
            }

            var viewModel = new TokenViewModel
            {
                Id = applicationUser.Id.ToString(),
                Username = applicationUser.UserName,
                AccessToken = securityResp.Data,
                Email = applicationUser.Email,
                NameSurname = applicationUser.NameSurname
            };

            apiResp.Data = viewModel;
            apiResp.Type = ResponseType.Success;

            return apiResp;
        }

        /// <summary>
        /// returns user info by identity manager
        /// </summary>
        /// <returns></returns>
        private async Task<ApiResponse<UserViewModel>> GetUser()
        {
            var user = await _accountService.GetUser(GetUserId().Value.ToString());

         //   var settings = _userBusiness.GetSettings(user.Settings);

            return new ApiResponse<UserViewModel>
            {
                Type = ResponseType.Success,
                Data = new UserViewModel
                {
                    Id = user.Id.ToString(),
                    CreatedAt = user.CreatedAt,
                    Email = user.Email,
                    NameSurname = user.NameSurname,
                    Username = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = user.Roles,
                    //Settings = new Settings
                    //{
                    //    FixedHeader = settings.FixedHeader,
                    //    OpenTagsView = settings.OpenTagsView,
                    //    ThemeColor = settings.ThemeColor,
                    //    PaginationAlign = settings.PaginationAlign
                    //}
                }
            };
        }

        /// <summary>
        /// creates new user
        /// </summary>
        /// <param name="model"></param>
        private async Task<ApiResponse> RegisterUser(RegisterViewModel model)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var now = DateTime.UtcNow;

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                NameSurname = model.NameSurname,
                UserName = model.Email,
                Roles = new List<string> { DatabaseKeys.ApplicationRoleName.User },
                EmailConfirmed = true,
               // Settings = _userBusiness.GetDefaultSettings(),
                CreatedAt = now
            };

            var resp = await _accountService.Register(applicationUser, model.Password);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }

        private async Task<ApiResponse> ResetAccount(string emailOrUsername)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var resp = await _accountService.ResetAccount(emailOrUsername);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }

        private async Task<ApiResponse> ResetUserPassword(ResetPasswordViewModel model)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var confirmResp = await _accountService.ConfirmPasswordReset(model.Password, model.SecurityCode);

            if (confirmResp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = confirmResp.ErrorCode;
                return apiResp;
            }

            var resp = await _accountService.ChangePassword(confirmResp.Data, model.Password);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }

        private ApiResponse UpdateSettings(SettingsViewModel model)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            //var userSettings = new UserSettings
            //{
            //    OpenTagsView = model.OpenTagsView.Value,
            //    FixedHeader = model.FixedHeader.Value,
            //    ThemeColor = model.ThemeColor,
            //    PaginationAlign = model.PaginationAlign
            //};

            //var resp = _userBusiness.UpdateSettings(GetUserId().Value, userSettings);

            //if (resp.Type != ResponseType.Success)
            //{
            //    apiResp.ErrorCode = resp.ErrorCode;
            //    return apiResp;
            //}

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }

        private async Task<ApiResponse> ChangePassword(string password)
        {
            var apiResp = new ApiResponse
            {
                Type = ResponseType.Fail
            };

            var resp = await _accountService.ChangePassword(GetUserId().Value, password);

            if (resp.Type != ResponseType.Success)
            {
                apiResp.ErrorCode = resp.ErrorCode;
                return apiResp;
            }

            apiResp.Type = ResponseType.Success;
            return apiResp;
        }
    }
}