using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository alertRepository;

        public AlertService(IAlertRepository alertRepository)
        {
            this.alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
        }

        public int AddAlert(IAlert alert)
        {
            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(backInStockAlert), Encoding.UTF8, "application/json");
                        content.GetType();

                        HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/BackInStockAlerts", content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        BackInStockAlert? result = JsonConvert.DeserializeObject<BackInStockAlert>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result.Id;
                    }
                    catch
                    {
                        return -1;
                    }

                case NewProductAlert newProductAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(newProductAlert), Encoding.UTF8, "application/json");
                        content.GetType();

                        HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/NewProductAlerts", content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        NewProductAlert? result = JsonConvert.DeserializeObject<NewProductAlert>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result.Id;
                    }
                    catch
                    {
                        return -1;
                    }

                case PriceDropAlert priceDropAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(priceDropAlert), Encoding.UTF8, "application/json");
                        content.GetType();

                        HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/PriceDropAlerts", content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        NewProductAlert? result = JsonConvert.DeserializeObject<NewProductAlert>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result.Id;
                    }
                    catch
                    {
                        return -1;
                    }

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            // return alertRepository.AddAlert(alert);
        }

        public bool DeleteAlert(int id, IAlert alert)
        {
            string link;

            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    link = "https://localhost:7040/api/BackInStockAlerts";
                    break;

                case NewProductAlert newProductAlert:
                    link = "https://localhost:7040/api/NewProductAlerts";
                    break;

                case PriceDropAlert priceDropAlert:
                    link = "https://localhost:7040/api/PriceDropAlerts";
                    break;

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync(link + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

            // return alertRepository.DeleteAlert(id, alert);
        }

        public IAlert? GetAlert(int id, IAlert alert)
        {
            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/BackInStockAlerts" + id)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        List<BackInStockAlert>? result = JsonConvert.DeserializeObject<List<BackInStockAlert>>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result[0];
                    }
                    catch
                    {
                        return null;
                    }

                case NewProductAlert newProductAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/NewProductAlerts" + id)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        List<NewProductAlert>? result = JsonConvert.DeserializeObject<List<NewProductAlert>>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result[0];
                    }
                    catch
                    {
                        return null;
                    }

                case PriceDropAlert priceDropAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/PriceDropAlerts" + id)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                        List<NewProductAlert>? result = JsonConvert.DeserializeObject<List<NewProductAlert>>(responseBody);
                        if (result == null)
                        {
                            throw new Exception("???");
                        }

                        return result[0];
                    }
                    catch
                    {
                        return null;
                    }

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            // alertRepository.GetAlert(alertId);
        }

        public IEnumerable<IAlert> GetAllAlerts()
        {
            List<IAlert> result = new List<IAlert>();

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/BackInStockAlerts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<BackInStockAlert> backInStockAlerts = JsonConvert.DeserializeObject<List<BackInStockAlert>>(responseBody);

                if (backInStockAlerts != null)
                {
                    result.AddRange(backInStockAlerts);
                }

                response = Task.Run(() => client.GetAsync("https://localhost:7040/api/NewProductAlerts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<NewProductAlert> newProductAlerts = JsonConvert.DeserializeObject<List<NewProductAlert>>(responseBody);

                if (newProductAlerts != null)
                {
                    result.AddRange(newProductAlerts);
                }

                response = Task.Run(() => client.GetAsync("https://localhost:7040/api/PriceDropAlerts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<PriceDropAlert> priceDropAlerts = JsonConvert.DeserializeObject<List<PriceDropAlert>>(responseBody);

                if (newProductAlerts != null)
                {
                    result.AddRange(newProductAlerts);
                }

                return result;
            }
            catch
            {
                return null;
            }

            // return alertRepository.GetAllAlerts();
        }

        public IEnumerable<IAlert> GetAllProductAlerts(int productId)
        {
            bool Selector(IAlert alert)
            {
                switch (alert)
                {
                    case NewProductAlert newProductAlert:
                        return newProductAlert.ProductId == productId;

                    case PriceDropAlert priceDropAlert:
                        return priceDropAlert.ProductId == productId;

                    case BackInStockAlert backInStockAlert:
                        return backInStockAlert.ProductId == productId;

                    default:
                        return false;
                }
            }

            // this is god awful
            return this.GetAllAlerts().Where(Selector);
        }

        public IEnumerable<IAlert> GetAllUserAlerts(int userId)
        {
            return this.GetAllAlerts().Where(
                alert =>
                {
                    return alert.UserId == userId;
                });
        }

        public bool UpdateAlert(int id, IAlert alert)
        {
            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(backInStockAlert), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/BackInStockAlerts" + id, content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                case NewProductAlert newProductAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(newProductAlert), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/NewProductAlerts" + id, content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                case PriceDropAlert priceDropAlert:
                    try
                    {
                        HttpClient client = new HttpClient();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(priceDropAlert), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/PriceDropAlerts" + id, content)).GetAwaiter().GetResult();
                        response.EnsureSuccessStatusCode();  // error-prone
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            // return alertRepository.UpdateAlert(id, alert);
        }
    }
}