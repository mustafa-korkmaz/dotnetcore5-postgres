using System.ComponentModel.DataAnnotations;

namespace dotnetpostgres.Api.ViewModels.User.Request
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [StringLength(50, ErrorMessage = ValidationErrorCode.BetweenLength, MinimumLength = 6)]
        [Display(Name ="PASSWORD")]
        public string Password { get; set; }
    }
}
