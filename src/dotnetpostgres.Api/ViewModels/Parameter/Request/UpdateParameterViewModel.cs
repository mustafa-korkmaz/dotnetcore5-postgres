using System.ComponentModel.DataAnnotations;

namespace dotnetpostgres.Api.ViewModels.Parameter.Request
{
    public class UpdateParameterViewModel
    {
        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [StringLength(100, ErrorMessage = ValidationErrorCode.BetweenLength, MinimumLength = AppConstant.MinimumLengthForSearch)]
        [Display(Name = "PARAMETER_VALUE")]
        public string Value { get; set; }

        [StringLength(250, ErrorMessage = ValidationErrorCode.MaxLength, MinimumLength = AppConstant.MinimumLengthForSearch)]
        [Display(Name = "PARAMETER_DESCRIPTION")]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [Range(0, byte.MaxValue, ErrorMessage = ValidationErrorCode.BetweenRange)]
        [Display(Name = "ORDER")]
        public byte? Order { get; set; }

        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [Range(1, int.MaxValue, ErrorMessage = ValidationErrorCode.BetweenRange)]
        [Display(Name = "PARAMETER_TYPE_ID")]
        public int? TypeId { get; set; }
    }
}
