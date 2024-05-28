using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IAlertService
    {
        int AddAlert(IAlert alert);
        bool DeleteAlert(int id, IAlert alert);
        bool UpdateAlert(int id, IAlert alert);
        IEnumerable<IAlert> GetAllAlerts();
        IAlert? GetAlert(int alertId, IAlert alert);
        IEnumerable<IAlert> GetAllUserAlerts(int userId);
        IEnumerable<IAlert> GetAllProductAlerts(int productId);
    }
}