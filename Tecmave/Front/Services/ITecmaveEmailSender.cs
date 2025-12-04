namespace Tecmave.Front.Services
{
    public interface ITecmaveEmailSender
    {
        Task SendAsync(string to, string subject, string bodyHtml);
    }
}
