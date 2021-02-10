using System;
using Microsoft.AspNetCore.Identity;

namespace dotnetpostgres.Dal.Entities.Identity
{
    public class ApplicationUserToken<T> : IdentityUserToken<T> where T : IEquatable<T>
    {
    }
}
