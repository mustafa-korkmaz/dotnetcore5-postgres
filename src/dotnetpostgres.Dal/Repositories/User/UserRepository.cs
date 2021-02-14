using System;
using System.Linq;
using dotnetpostgres.Dal.Databases.Postgres;
using Microsoft.EntityFrameworkCore;

namespace dotnetpostgres.Dal.Repositories.User
{
    public class UserRepository : PostgreSqlDbRepository<Entities.Identity.ApplicationUser, Guid>, IUserRepository
    {
        public UserRepository(PostgresDbContext context) : base(context)
        {

        }

        public Guid? GetUserIdByEmail(string email)
        {
            email = email.ToUpperInvariant();
            var user = Entities.AsNoTracking().FirstOrDefault(p => p.NormalizedEmail == email);

            return user?.Id;
        }
    }
}
