using System;
using dotnetpostgres.Dto.User;

namespace dotnetpostgres.Services.User
{
    public interface IUserService
    {
        Response.Response UpdateSettings(Guid userId, UserSettings newSettings);
    }
}
