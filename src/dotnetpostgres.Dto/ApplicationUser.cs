using System;
using System.Collections.Generic;
using System.Text.Json;
using dotnetpostgres.Dto.User;

namespace dotnetpostgres.Dto
{
    public class ApplicationUser : IDto<Guid>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string NameSurname { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string ImageName { get; set; }

        public string PhoneNumber { get; set; }

        public string RawSettings { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<string> Roles { get; set; }

        public UserSettings Settings
        {
            get
            {
                if (string.IsNullOrEmpty(RawSettings))
                {
                    return GetDefaultSettings();
                }

                return JsonSerializer.Deserialize<UserSettings>(RawSettings);
            }
        }

        public Dictionary<string, string> Claims { get; set; }

        private UserSettings GetDefaultSettings()
        {
            // default theme settings
            return new()
            {
                OpenTagsView = true,
                FixedHeader = false,
                ThemeColor = "#1890FF",
                PaginationAlign = "left"
            };
        }
    }


}
