using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IAlertRepository
    {
        int AddAlert(IAlert alert);
        bool DeleteAlert(int id, IAlert alert);
        bool UpdateAlert(int id, IAlert alert);
        IEnumerable<IAlert> GetAllAlerts();
        IAlert? GetAlert(int alertId);
        IEnumerable<IAlert> GetAllUserAlerts(int userId);
        IEnumerable<IAlert> GetAllProductAlerts(int productId);
    }
}
