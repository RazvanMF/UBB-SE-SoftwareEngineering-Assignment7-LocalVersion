using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string connectionString;

        public ProductRepository(IConfigurationManager configurationManager)
        {
            connectionString = configurationManager.GetConnectionString("appsettings.json");
        }

        // For a product, the list of attributes is represented in the database as a string of key-value pair separated by
        // semicolons. e.g. (In the DB) "colour:red;weight:200g;battery-life:2h".
        // Here in the repository this string is translated to a dictionary.
        public int AddProduct(Product product)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Product (name,category,description,brand,imageURL,attributes) VALUES (@name,@category,@description,@brand,@imageURL,@attributes); SELECT SCOPE_IDENTITY();";

            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@category", product.Category);
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@brand", product.Brand);
            command.Parameters.AddWithValue("@imageURL", product.ImageURL);
            command.Parameters.AddWithValue("@attributes", ConvertAttributesFromDictToString(product.Attributes));

            int newProductID = Convert.ToInt32(command.ExecuteScalar());
            return newProductID;
        }

        public bool DeleteProduct(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM Product WHERE Product.id = @id";

            command.Parameters.AddWithValue("@id", id);
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Product";

            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                Product product = new ()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Category = reader.GetString(2),
                    Description = reader.GetString(3),
                    Brand = reader.GetString(4),
                    ImageURL = reader.GetString(5),
                    Attributes = ConvertAttributesFromStringToDict(reader.GetString(6))
                };

                products.Add(product);
            }

            return products;
        }

        public Product? GetProduct(int id)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM Product WHERE Product.id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Product product = new ()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Category = reader.GetString(2),
                    Description = reader.GetString(3),
                    Brand = reader.GetString(4),
                    ImageURL = reader.GetString(5),
                    Attributes = ConvertAttributesFromStringToDict(reader.GetString(6))
                };

                return product;
            }

            return null;
        }

        public bool UpdateProduct(int id, Product product)
        {
            using SqlConnection connection = new (connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Product " +
                                  "SET name = @name, category = @category, description = @description, brand = @brand, imageURL = @imageURL, " +
                                  "attributes = @attributes WHERE id = @id";

            command.Parameters.AddWithValue("@name", product.Name);
            command.Parameters.AddWithValue("@category", product.Category);
            command.Parameters.AddWithValue("@description", product.Description);
            command.Parameters.AddWithValue("@brand", product.Brand);
            command.Parameters.AddWithValue("@imageURL", product.ImageURL);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@attributes", ConvertAttributesFromDictToString(product.Attributes));

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public static IDictionary<string, string> ConvertAttributesFromStringToDict(string string_attributes)
        {
            if (string.IsNullOrEmpty(string_attributes))
            {
                throw new ArgumentException("string_attributes cannot be null or empty");
            }

            const int KEY = 0, VALUE = 1;
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            IEnumerable<string> split_attributes = string_attributes.Split(';');

            foreach (string split_attribute in split_attributes)
            {
                string[] keyValue = split_attribute.Split(':');
                if (keyValue.Length == 2)
                {
                    attributes.Add(keyValue[KEY], keyValue[VALUE]);
                }
            }

            return attributes;
        }

        public static string ConvertAttributesFromDictToString(IDictionary<string, string> dictionary_attributes)
        {
            StringBuilder stringBuilder = new ();

            foreach (KeyValuePair<string, string> pair in dictionary_attributes)
            {
                stringBuilder.Append(pair.Key + ':' + pair.Value + ';');
            }

            return stringBuilder.ToString();
        }
    }
}
