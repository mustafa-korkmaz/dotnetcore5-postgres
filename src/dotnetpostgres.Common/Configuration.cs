
namespace dotnetpostgres.Common
{
    /// <summary>
    /// appsettings.json file or env vars model class
    /// </summary>
    public class Configuration
    {
        public string ApiUrl { get; set; }

        public string PfeWebConnection { get; set; }

        public string CacheProvider { get; set; }

        public string AdminPanelUrl { get; set; }

        public LoggingSettings Logging { get; set; }

        public EMailSettings Email { get; set; }
    }

    public class LoggingSettings
    {
        public bool IsActive { get; set; }

        public string InfoPath { get; set; }

        public string InfoFile { get; set; }

        public string ErrorPath { get; set; }

        public string ErrorFile { get; set; }

        public string ReqAndRespPath { get; set; }

        public string ReqAndRespFile { get; set; }
    }

    public class EMailSettings
    {
        public bool UseSsl { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public int MaximumCount { get; set; }

        public string TemplatePath { get; set; }

        public string MailFromForSystem { get; set; }

        public string MailDisplayNameForSystem { get; set; }

        public string MailPassForSystem { get; set; }

        public string MailFromForTeachers { get; set; }

        public string MailDisplayNameForTeachers { get; set; }

        public string MailPassforTeachers { get; set; }

        public string MailFromForScholarship { get; set; }

        public string MailDisplayNameForScholarship { get; set; }

        public string MailPassforScholarship { get; set; }
    }

    public class SmsSettings
    {
        /// <summary>
        /// sms sending url
        /// </summary>
        public string SendSmsUrl { get; set; }

        /// <summary>
        /// value of Authorization header
        /// </summary>
        public string AuthorizationHeader { get; set; }
    }
}
