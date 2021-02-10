using System;
using Microsoft.AspNetCore.Identity;

namespace dotnetpostgres.Dal.Entities.Identity
{
    public class ApplicationRoleClaim<T> : IdentityRoleClaim<T> where T : IEquatable<T>
    {
    }
}
