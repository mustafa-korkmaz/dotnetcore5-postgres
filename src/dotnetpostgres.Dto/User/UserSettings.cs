
using System.Text.Json.Serialization;

namespace dotnetpostgres.Dto.User
{
    public class UserSettings
    {
        [JsonPropertyName("tc")]
        public string ThemeColor { get; set; }

        [JsonPropertyName("pa")]
        public string PaginationAlign { get; set; }

        [JsonPropertyName("otv")]
        public bool OpenTagsView { get; set; }

        [JsonPropertyName("fh")]
        public bool FixedHeader { get; set; }
    }
}
