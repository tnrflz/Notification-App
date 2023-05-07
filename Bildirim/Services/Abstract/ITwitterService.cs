using Bildirim.Models;

namespace Bildirim.Services.Abstract
{
    public interface ITwitterService
    {
        Task SendAsync( );

        Task GetAccount();
    }
}
