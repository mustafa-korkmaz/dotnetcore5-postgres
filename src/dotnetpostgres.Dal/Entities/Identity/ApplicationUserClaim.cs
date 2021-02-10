using System;
using Microsoft.AspNetCore.Identity;

namespace dotnetpostgres.Dal.Entities.Identity
{
    public class ApplicationUserClaim<T> : IdentityUserClaim<T> where T : IEquatable<T>
    {
    }
}
