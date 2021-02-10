using System.ComponentModel.DataAnnotations;
using dotnetpostgres.Common;

namespace dotnetpostgres.Api.ViewModels
{
    public class IdViewModel
    {
        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [Range(1, int.MaxValue, ErrorMessage = ValidationErrorCode.BetweenRange)]
        [Display(Name = "ID")]
        public int Id { get; set; }
    }
}