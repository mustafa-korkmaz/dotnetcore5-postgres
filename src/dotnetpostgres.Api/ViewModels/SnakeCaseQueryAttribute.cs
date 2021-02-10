using dotnetpostgres.Common;
using Microsoft.AspNetCore.Mvc;

namespace dotnetpostgres.Api.ViewModels
{
    public class SnakeCaseQueryAttribute : FromQueryAttribute
    {
        public SnakeCaseQueryAttribute(string name)
        {
            Name = name.ToSnakeCase();
        }
    }
}
