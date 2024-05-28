using System.Data;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly string connectionString;

        public AlertRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        public int AddAlert(IAlert alert)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    command.CommandText = @"
                INSERT INTO BackInStockAlerts (UserId, ProductId, MarketplaceId) 
                VALUES (@UserId, @ProductId, @MarketplaceId); 
                SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@UserId", backInStockAlert.UserId);
                    command.Parameters.AddWithValue("@ProductId", backInStockAlert.ProductId);
                    command.Parameters.AddWithValue("@MarketplaceId", backInStockAlert.MarketplaceId);
                    break;

                case NewProductAlert newProductAlert:
                    command.CommandText = @"
                INSERT INTO NewProductAlerts (UserId, ProductId) 
                VALUES (@UserId, @ProductId); 
                SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@UserId", newProductAlert.UserId);
                    command.Parameters.AddWithValue("@ProductId", newProductAlert.ProductId);
                    break;

                case PriceDropAlert priceDropAlert:
                    command.CommandText = @"
                INSERT INTO PriceDropAlerts (UserId, ProductId, OldPrice, NewPrice) 
                VALUES (@UserId, @ProductId, @OldPrice, @NewPrice); 
                SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@UserId", priceDropAlert.UserId);
                    command.Parameters.AddWithValue("@ProductId", priceDropAlert.ProductId);
                    command.Parameters.AddWithValue("@OldPrice", priceDropAlert.OldPrice);
                    command.Parameters.AddWithValue("@NewPrice", priceDropAlert.NewPrice);
                    break;

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            int newAlertId = Convert.ToInt32(command.ExecuteScalar());
            return newAlertId;
        }

        public bool DeleteAlert(int id, IAlert alert)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            switch (alert)
            {
                case BackInStockAlert backInStockAlert:
                    command.CommandText = "DELETE FROM BackInStockAlerts WHERE BackInStockAlerts.id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    break;

                case NewProductAlert newProductAlert:
                    command.CommandText = "DELETE FROM NewProductAlerts WHERE NewProductAlerts.id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    break;

                case PriceDropAlert priceDropAlert:
                    command.CommandText = "DELETE FROM PriceDropAlerts WHERE PriceDropAlerts.id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    break;

                default:
                    throw new NotImplementedException("Unsupported alert type.");
            }

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IAlert? GetAlert(int alertId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAlert> GetAllAlerts()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            // Get BackInStockAlerts
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM BackInStockAlerts";

            SqlDataReader reader = command.ExecuteReader();
            List<IAlert> alerts = new List<IAlert>();

            while (reader.Read())
            {
                BackInStockAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    MarketplaceId = reader.GetInt32(3),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get NewProductAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM NewProductAlerts";
            command.CommandType = CommandType.Text;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                NewProductAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get PriceDropAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PriceDropAlerts";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                PriceDropAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    OldPrice = (float)decimal.ToDouble(reader.GetDecimal(3)),
                    NewPrice = (float)decimal.ToDouble(reader.GetDecimal(4)),
                };

                alerts.Add(alert);
            }

            reader.Close();

            return alerts;
        }

        public IEnumerable<IAlert> GetAllProductAlerts(int productId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            // Get BackInStockAlerts
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM BackInStockAlerts WHERE ProductId = @productId";
            command.Parameters.AddWithValue("@productId", productId);

            SqlDataReader reader = command.ExecuteReader();
            List<IAlert> alerts = new List<IAlert>();

            while (reader.Read())
            {
                BackInStockAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    MarketplaceId = reader.GetInt32(3),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get NewProductAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM NewProductAlerts WHERE ProductId = @productId";
            command.Parameters.AddWithValue("@productId", productId);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                NewProductAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get PriceDropAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PriceDropAlerts WHERE ProductId = @productId";
            command.Parameters.AddWithValue("@productId", productId);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                PriceDropAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    OldPrice = (float)decimal.ToDouble(reader.GetDecimal(3)),
                    NewPrice = (float)decimal.ToDouble(reader.GetDecimal(4)),
                };

                alerts.Add(alert);
            }

            reader.Close();

            return alerts;
        }

        public IEnumerable<IAlert> GetAllUserAlerts(int userId)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            // Get BackInStockAlerts
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM BackInStockAlerts WHERE UserId = @userId";
            command.Parameters.AddWithValue("@UserId", userId);

            SqlDataReader reader = command.ExecuteReader();
            List<IAlert> alerts = new List<IAlert>();

            while (reader.Read())
            {
                BackInStockAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    MarketplaceId = reader.GetInt32(3),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get NewProductAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM NewProductAlerts WHERE UserId = @userId";
            command.Parameters.AddWithValue("@UserId", userId);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                NewProductAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                };

                alerts.Add(alert);
            }

            reader.Close();

            // Get PriceDropAlerts
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PriceDropAlerts WHERE UserId = @userId";
            command.Parameters.AddWithValue("@UserId", userId);

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                PriceDropAlert alert = new ()
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    OldPrice = (float)decimal.ToDouble(reader.GetDecimal(3)),
                    NewPrice = (float)decimal.ToDouble(reader.GetDecimal(4)),
                };

                alerts.Add(alert);
            }

            reader.Close();

            return alerts;
        }

        public bool UpdateAlert(int id, IAlert alert)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;

            if (alert is BackInStockAlert backInStockAlert)
            {
                command.CommandText = "UPDATE BackInStockAlerts " +
                                      "SET UserId = @UserId, " +
                                      "ProductId = @ProductId, " +
                                      "MarketplaceId = @MarketplaceId " +
                                      "WHERE id = @Id";
                command.Parameters.AddWithValue("@UserId", backInStockAlert.UserId);
                command.Parameters.AddWithValue("@ProductId", backInStockAlert.ProductId);
                command.Parameters.AddWithValue("@MarketplaceId", backInStockAlert.MarketplaceId);
                command.Parameters.AddWithValue("@Id", id);
            }
            else if (alert is NewProductAlert newProductAlert)
            {
                command.CommandText = "UPDATE NewProductAlerts " +
                                      "SET UserId = @UserId, " +
                                      "ProductId = @ProductId " +
                                      "WHERE id = @Id";
                command.Parameters.AddWithValue("@UserId", newProductAlert.UserId);
                command.Parameters.AddWithValue("@ProductId", newProductAlert.ProductId);
                command.Parameters.AddWithValue("@Id", id);
            }
            else if (alert is PriceDropAlert priceDropAlert)
            {
                command.CommandText = "UPDATE PriceDropAlerts " +
                                      "SET UserId = @UserId, " +
                                      "ProductId = @ProductId, " +
                                      "OldPrice = @OldPrice, " +
                                      "NewPrice = @NewPrice " +
                                      "WHERE id = @Id";
                command.Parameters.AddWithValue("@UserId", priceDropAlert.UserId);
                command.Parameters.AddWithValue("@ProductId", priceDropAlert.ProductId);
                command.Parameters.AddWithValue("@OldPrice", priceDropAlert.OldPrice);
                command.Parameters.AddWithValue("@NewPrice", priceDropAlert.NewPrice);
                command.Parameters.AddWithValue("@Id", id);
            }
            else
            {
                throw new NotImplementedException("Unsupported alert type.");
            }

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
}
