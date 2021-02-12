using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dotnetpostgres.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly Settings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly IOptions<Configuration> _emailConfig;

        public EmailService(IOptions<Configuration> emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;

            _emailSettings = new Settings
            {
                Port = emailConfig.Value.Email.Port,
                UseSsl = emailConfig.Value.Email.UseSsl,
                Host = emailConfig.Value.Email.Host
            };
        }

        public bool SendEmail(Email email)
        {
            if (string.IsNullOrEmpty(email.From))
            {
                email.From = _emailConfig.Value.Email.MailFrom;
                email.DisplayName = _emailConfig.Value.Email.MailDisplayName;
                email.Password = _emailConfig.Value.Email.MailPassword;
            }

            if (!email.Template.Variables.ContainsKey("admin_panel_url"))
            {
                email.Template.Variables.Add("admin_panel_url", _emailConfig.Value.AdminPanelUrl);
            }

            if (!email.Template.Variables.ContainsKey("landing_page_url"))
            {
                email.Template.Variables.Add("landing_page_url", _emailConfig.Value.LandingPageUrl);
            }

            var mailHandler = MailHandler.Instance;

            mailHandler.Settings = _emailSettings;

            mailHandler.Send(email);

            if (mailHandler.HasError)
            {
                _logger.LogError($"sending to {email.To} failed.", mailHandler.GetException());
            }

            return !mailHandler.HasError;
        }
    }
}