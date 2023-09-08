using Npgsql;
using Bogus;

namespace FakeDataGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connString = "Host=127.0.0.1;Port=5432;Username=usr;Password=passwd;Database=db";
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    // Create the productCategory table
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS productCategory (
                            Id SERIAL PRIMARY KEY,
                            Name VARCHAR(255) NOT NULL
                        )";
                    await cmd.ExecuteNonQueryAsync();

                    // Initialize Bogus generator for categories
                    var categoryFaker = new Faker();
                    var categoryIds = new List<int>();

                    // Insert 1,000 random categories into the productCategory table
                    for (int i = 0; i < 1000; i++)
                    {
                        string categoryName = categoryFaker.Commerce.Department();
                        cmd.CommandText = @"
                            INSERT INTO productCategory (Name) VALUES (@categoryName) RETURNING Id";
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);

                        // Get the generated category Id and store it
                        int categoryId = (int)await cmd.ExecuteScalarAsync();
                        categoryIds.Add(categoryId);
                        cmd.Parameters.Clear();
                    }

                    // Create the products table
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS products (
                            id SERIAL PRIMARY KEY,
                            name VARCHAR(255),
                            description TEXT,
                            price NUMERIC(10, 2),
                            stock_quantity INT,
                            category_id INT,
                            created_at TIMESTAMP DEFAULT NOW(),
                            FOREIGN KEY (category_id) REFERENCES productCategory (Id)
                        )";
                    await cmd.ExecuteNonQueryAsync();

                    // Initialize Bogus generator for products
                    var productFaker = new Faker();
                    var random = new Random();

                    // Insert 10,000 random products
                    for (int i = 0; i < 10000; i++)
                    {
                        var product = new
                        {
                            name = productFaker.Commerce.ProductName(),
                            description = productFaker.Lorem.Sentence(),
                            price = Math.Round((decimal)productFaker.Random.Double(10, 1000), 2),
                            stock_quantity = productFaker.Random.Int(1, 1000),
                            category_id = categoryIds[random.Next(0, 1000)] // Assign a random category Id
                        };

                        cmd.CommandText = @"
                            INSERT INTO products (name, description, price, stock_quantity, category_id) 
                            VALUES (@name, @description, @price, @stock_quantity, @category_id)";

                        cmd.Parameters.AddWithValue("@name", product.name);
                        cmd.Parameters.AddWithValue("@description", product.description);
                        cmd.Parameters.AddWithValue("@price", product.price);
                        cmd.Parameters.AddWithValue("@stock_quantity", product.stock_quantity);
                        cmd.Parameters.AddWithValue("@category_id", product.category_id);

                        await cmd.ExecuteNonQueryAsync();
                        cmd.Parameters.Clear();
                    }
                }

                Console.WriteLine("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}