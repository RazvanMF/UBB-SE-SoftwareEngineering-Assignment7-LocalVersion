using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class AlertController
    {
        private readonly IAlertService alertService;

        public AlertController(IAlertService alertService)
        {
            this.alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
        }

        public int AddAlert(IAlert alert)
        {
            return alertService.AddAlert(alert);
        }

        public bool DeleteAlert(int id, IAlert alert)
        {
            return alertService.DeleteAlert(id, alert);
        }

        public IAlert? GetAlert(int alertId, IAlert alert) => alertService.GetAlert(alertId, alert);

        public IEnumerable<IAlert> GetAllAlerts()
        {
            return alertService.GetAllAlerts();
        }

        public IEnumerable<IAlert> GetAllProductAlerts(int productId)
        {
            return alertService.GetAllProductAlerts(productId);
        }

        public IEnumerable<IAlert> GetAllUserAlerts(int userId)
        {
            return alertService.GetAllUserAlerts(userId);
        }

        public bool UpdateAlert(int id, IAlert alert)
        {
            return alertService.UpdateAlert(id, alert);
        }
    }
}