﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using dotnetpostgres.Dto;
using dotnetpostgres.Response;
using dotnetpostgres.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace dotnetpostgres.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Dal.Entities.Identity.ApplicationUser> _userManager;
        private readonly RoleManager<Dal.Entities.Identity.ApplicationRole> _roleManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        //private readonly ISlackService _slackService;

        public AccountService(
            UserManager<Dal.Entities.Identity.ApplicationUser> userManager,
            RoleManager<Dal.Entities.Identity.ApplicationRole> roleManager,
            ILogger<AccountService> logger, IMapper mapper, IEmailService emailService /*ISlackService slackService*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
            _emailService = emailService;
            //_slackService = slackService;
        }

        public async Task<DataResponse<string>> GetToken(ApplicationUser userDto, string password)
        {
            var resp = new DataResponse<string>
            {
                Type = ResponseType.Fail
            };

            Dal.Entities.Identity.ApplicationUser user;

            if (userDto.UserName.Contains("@")) // login via email
            {
                user = await _userManager.FindByEmailAsync(userDto.Email);
            }
            else // login via username
            {
                user = await _userManager.FindByNameAsync(userDto.UserName);
            }

            if (user == null)
            {
                resp.ErrorCode = ErrorCode.UserNotFound;
                resp.Type = ResponseType.RecordNotFound;
                return resp;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                resp.ErrorCode = ErrorCode.IncorrectUsernameOrPassword;
                return resp;
            }

            //get user roles
            var roles = await _userManager.GetRolesAsync(user);

            userDto.Email = user.Email;
            userDto.PhoneNumber = user.PhoneNumber;
            userDto.Id = user.Id;
            userDto.Roles = roles;

            var token = GenerateToken(userDto);

            resp.Data = token;
            resp.Type = ResponseType.Success;

            return resp;
        }

        public async Task<Response.Response> Register(ApplicationUser userDto, string password)
        {
            var resp = new Response.Response { Type = ResponseType.Fail };

            var userByName = await _userManager.FindByNameAsync(userDto.UserName);

            if (userByName != null)
            {
                resp.ErrorCode = ErrorCode.UserExists;
                return resp;
            }

            var userByEmail = await _userManager.FindByEmailAsync(userDto.Email);

            if (userByEmail != null)
            {
                resp.ErrorCode = ErrorCode.UserExists;
                return resp;
            }

            //start sending slack notification
            //  var slackMessageTask = _slackService.SendMessage($"{userDto.Email} joined :tada:", "account-tracker");

            var userModel = new Dal.Entities.Identity.ApplicationUser
            {
                Id = userDto.Id,
                Email = userDto.Email ?? "",
                EmailConfirmed = userDto.EmailConfirmed,
                UserName = userDto.UserName,
                NameSurname = userDto.NameSurname,
                PasswordHash = HashPassword(password),
                SecurityStamp = Guid.NewGuid().ToString(),
                // Settings = userDto.Settings,
                CreatedAt = userDto.CreatedAt,
            };

            await _userManager.CreateAsync(userModel);

            userDto.Id = userModel.Id;

            await _userManager.AddToRoleAsync(userModel, userDto.Roles.First());

            resp.Type = ResponseType.Success;

            //await slackMessageTask;

            return resp;
        }

        public async Task<Response.Response> ResetAccount(string emailOrUsername)
        {
            var resp = new Response.Response
            {
                Type = ResponseType.Fail
            };

            var user = await _userManager.FindByEmailAsync(emailOrUsername);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(emailOrUsername);
            }

            if (user == null)
            {
                resp.ErrorCode = ErrorCode.UserNotFound;
                return resp;
            }

            var now = DateTime.UtcNow;
            var unixTimestamp = Utility.GetUnixTimeStamp(now);

            var resetLink = Utility.Base64Encode($"{user.Id:N}::{user.SecurityStamp}::{unixTimestamp}");

            var mailSent = SendResetPasswordEmail(resetLink, user.Email);

            if (!mailSent)
            {
                resp.ErrorCode = ErrorCode.ApplicationException;
                return resp;
            }

            user.LockoutEnd = now;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation(string.Format(LoggingOperationPhrase.PasswordReset, user.Id));

            resp.Type = ResponseType.Success;

            return resp;
        }

        public async Task<DataResponse<Guid>> ConfirmPasswordReset(string password, string securityCode)
        {
            var resp = new DataResponse<Guid>
            {
                Type = ResponseType.Fail,
                Data = Guid.Empty
            };

            string decodedLink;

            try
            {
                decodedLink = Utility.Base64Decode(securityCode);
            }
            catch (Exception e)
            {
                _logger.LogError($"{securityCode} decode error", e);
                resp.ErrorCode = ErrorCode.ApplicationException;
                return resp;
            }

            var linkParams = decodedLink.Split("::");

            if (linkParams.Length != 3)
            {
                _logger.LogError($"{securityCode} params cannot be found");

                resp.ErrorCode = ErrorCode.ApplicationException;
                return resp;
            }

            var userId = Guid.Parse(linkParams[0]);
            var securityStamp = linkParams[1];
            var unixTimestamp = int.Parse(linkParams[2]);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || user.SecurityStamp != securityStamp)
            {
                resp.ErrorCode = ErrorCode.SecurityError;
                return resp;
            }

            var lockoutEndDate = user.LockoutEnd.Value.UtcDateTime;

            if (unixTimestamp != Utility.GetUnixTimeStamp(lockoutEndDate))
            {
                resp.ErrorCode = ErrorCode.SecurityCodeExpired;
                return resp;
            }

            if (lockoutEndDate.AddMinutes(10) < DateTime.UtcNow)
            {
                resp.ErrorCode = ErrorCode.SecurityCodeExpired;
                return resp;
            }

            resp.Data = userId;
            resp.Type = ResponseType.Success;

            return resp;
        }

        public async Task<Response.Response> ChangePassword(Guid userId, string newPassword)
        {
            var resp = new Response.Response
            {
                Type = ResponseType.Fail
            };

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                resp.ErrorCode = ErrorCode.UserNotFound;
                resp.Type = ResponseType.RecordNotFound;

                return resp;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, token, newPassword);

            //log user password reset request
            _logger.LogInformation(string.Format(LoggingOperationPhrase.PasswordChanged, user.Id));

            resp.Type = ResponseType.Success;

            return resp;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);

            var claims = await _userManager.GetClaimsAsync(userEntity);
            var userClaims = claims.ToList();

            var userRoles = await _userManager.GetRolesAsync(userEntity);

            var roles = _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name))
                .ToList();

            foreach (var role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                userClaims.AddRange(roleClaims);
            }

            var userDto = _mapper.Map<Dal.Entities.Identity.ApplicationUser, ApplicationUser>(userEntity);

            userDto.Roles = userRoles;
            userDto.Claims = userClaims.ToDictionary(c => c.Type, c => c.Value);

            return userDto;
        }

        public async Task<Guid?> GetUserId(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user?.Id;
        }

        #region private methods

        private string GenerateToken(ApplicationUser user)
        {
            var handler = new JwtSecurityTokenHandler();

            List<Claim> claims = new List<Claim>();

            foreach (var userRole in user.Roles)
            {
                var roleIdentifierClaim = new Claim(ClaimTypes.Role, userRole, ClaimValueTypes.String);

                claims.Add(roleIdentifierClaim);
            }

            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String);
            var emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String);
            var phoneClaim = new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "", ClaimValueTypes.String);

            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim("username", user.UserName));

            claims.Add(nameIdentifierClaim);
            claims.Add(emailClaim);
            claims.Add(phoneClaim);

            //foreach (var userClaim in user.Claims)
            //{
            //    var id = GetClaimId(userClaim.Value);
            //    claims.Add(new Claim(string.Format(":{0}:", id), id));
            //}

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.Email, "Token"), claims);

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = JwtTokenConstants.Issuer,
                Audience = JwtTokenConstants.Audience,
                SigningCredentials = JwtTokenConstants.SigningCredentials,
                Subject = identity,
                Expires = DateTime.Now.Add(JwtTokenConstants.TokenExpirationTime),
                NotBefore = DateTime.Now
            });

            return handler.WriteToken(securityToken);
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private string GetClaimId(string value)
        {
            var id = value.Split("_")[0];
            return id;
        }

        private bool SendResetPasswordEmail(string resetLink, string emailAddress)
        {
            var email = new Email.Email
            {
                To = emailAddress,
                Subject = "Şifre sıfırlama isteği",
                Template = new Template
                {
                    Name = "reset-password-link.html",
                    Variables = new Dictionary<string, string>
                    {
                        {"reset_link", resetLink}
                    }
                }
            };

            return _emailService.SendEmail(email);
        }

        #endregion private methods
    }
}


