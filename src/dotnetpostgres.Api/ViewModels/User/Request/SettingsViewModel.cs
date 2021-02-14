using System.ComponentModel.DataAnnotations;

namespace dotnetpostgres.Api.ViewModels.User.Request
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [Display(Name = "OPEN_TAGS_VIEW")]
        public bool? OpenTagsView { get; set; }

        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [Display(Name = "FIXED_HEADER")]
        public bool? FixedHeader { get; set; }

        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [StringLength(7, ErrorMessage = ValidationErrorCode.ExactLength, MinimumLength = 7)]
        [Display(Name = "THEME_COLOR")]
        public string ThemeColor { get; set; }

        [Required(ErrorMessage = ValidationErrorCode.RequiredField)]
        [StringLength(5, ErrorMessage = ValidationErrorCode.BetweenLength, MinimumLength = 4)]
        [Display(Name = "PAGINATION_ALIGN")]
        public string PaginationAlign { get; set; }
    }
}
