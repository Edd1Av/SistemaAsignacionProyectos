namespace WEB.Services.Mails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string name, string message, string url);
    }
}
