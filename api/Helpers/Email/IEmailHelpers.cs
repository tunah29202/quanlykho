namespace Helpers.Email
{
    public interface IEmailHelpers
    {
        bool SendEmailWithBody(string body, string emailTo, string subject);
    }
}
