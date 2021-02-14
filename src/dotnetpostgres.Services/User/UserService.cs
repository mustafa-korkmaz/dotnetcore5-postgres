using System;
using System.Text.Json;
using AutoMapper;
using dotnetpostgres.Dal;
using dotnetpostgres.Dal.Repositories.User;
using dotnetpostgres.Dto.User;
using Microsoft.Extensions.Logging;

namespace dotnetpostgres.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, IMapper mapper, IUnitOfWork uow)
        {
            _logger = logger;
            _mapper = mapper;
            _uow = uow;
            _repository = _uow.Repository<IUserRepository, Dal.Entities.Identity.ApplicationUser>();
        }

    

        public Response.Response UpdateSettings(Guid userId, UserSettings newSettings)
        {
            var resp = new Response.Response
            {
                Type = ResponseType.Fail
            };

            var user = _repository.GetById(userId);

            if (user == null)
            {
                resp.ErrorCode = ErrorCode.UserNotFound;
                return resp;
            }

            user.Settings = JsonSerializer.Serialize(newSettings);

            if (user.Settings.Length > 250)
            {
                resp.ErrorCode = ErrorCode.ObjectExceededMaxAllowedLength;
                return resp;
            }

            _repository.Update(user);

            _uow.Save();

            resp.Type = ResponseType.Success;

            return resp;

        }
    }
}
