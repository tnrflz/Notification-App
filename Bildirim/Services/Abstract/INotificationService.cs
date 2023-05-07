using Bildirim.Models;

namespace Bildirim.Services.Abstract
{
    public interface INotificationService
    {

        Task<ResponseModel> SendNotification(NotificationModel notificationModel);

    }
}
