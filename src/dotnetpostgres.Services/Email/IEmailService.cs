
namespace dotnetpostgres.Services.Email
{
    public interface IEmailService
    {
        bool SendEmail(Email email);
    }
}