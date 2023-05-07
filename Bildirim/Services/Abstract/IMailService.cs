using Bildirim.Models;

namespace Bildirim.Services.Abstract
{
    public interface IMailService
    {
        Task SendAsync(MailModel model);


    }
}
