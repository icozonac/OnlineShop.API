using System.Data.SqlClient;
using OnlineShop.API.Models;




namespace OnlineShop.API.Controllers.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string dbConnection;
        private readonly string dateformat;

        public DataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbConnection = configuration.GetConnectionString("DefaultConnection");
            dateformat = configuration.GetSection("DateFormat").Value;
        }

        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = new List<ProductCategory>();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM ProductCategories;";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new ProductCategory
                    {
                        Id = (int)reader["CategoryId"],
                        Category = (string)reader["Category"],
                        SubCategory = (string)reader["SubCategory"],


                    };
                    productCategories.Add(category);
                }

            }
            return productCategories;
        }


        public Offer GetOffer(int id)
        {
            var offer = new Offer();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM Offers WHERE OfferID=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    offer.Id = (int)reader["OfferId"];
                    offer.Title = (string)reader["Title"];
                    offer.Discount = (int)reader["Discount"];
                }   

            }
                return offer;
            
        }

        public ProductCategory GetProductCategory(int id)
        {
            var productCategory = new ProductCategory();

            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM ProductCategories WHERE CategoryId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productCategory.Id = (int)reader["CategoryId"];
                    productCategory.Category = (string)reader["Category"];
                    productCategory.SubCategory = (string)reader["SubCategory"];
                }
            }
            return productCategory;
        }

        public List<Product> GetProducts(string category, string subcategory, int count)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT TOP " + count + "* FROM Products WHERE CategoryId=(SELECT CategoryId FROM ProductCategories WHERE Category=@c AND SubCategory=@s) ORDER BY newid();";
                command.CommandText = query;
                command.Parameters.Add("@c", System.Data.SqlDbType.NVarChar).Value = category;
                command.Parameters.Add("@s", System.Data.SqlDbType.NVarChar).Value = subcategory;


                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"]


                    };
                    var categoryid = (int)reader["CategoryId"];
                    product.ProductCategory = GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer =GetOffer(offerid);

                    products.Add(product);
                }

            }
            return products;
        }

        public Product GetProduct(int id)
        {
            var product = new Product();
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM Products WHERE ProductId=" + id + ";";
                command.CommandText = query;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    product.Id = (int)reader["ProductId"];
                    product.Title = (string)reader["Title"];
                    product.Description = (string)reader["Description"];
                    product.Price = (double)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                    product.ImageName = (string)reader["ImageName"];

                    var categoryid = (int)reader["CategoryId"];
                    product.ProductCategory = GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerid);
                }
            }
            return product;
        }
        
        public bool InsertUser(User user)
        {
            using (SqlConnection connection = new(dbConnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT COUNT(*) FROM Users WHERE Email='" + user.Email + "';";
                command.CommandText = query;

                int count = (int)command.ExecuteScalar();
                if(count > 0)
                {
                    connection.Close();
                    return false;
                }

                query = "INSERT INTO Users(FirstName, LastName, Adress, Mobile, Email, Password, CreatedAt, ModifiedAt) values (@fn, @ln, @add, @mb, @em, @pwd, @cat, @mat); ";
                command.CommandText = query;

                command.Parameters.Add("@fn", System.Data.SqlDbType.NVarChar).Value = user.FirstName;
                command.Parameters.Add("@ln", System.Data.SqlDbType.NVarChar).Value = user.LastName;
                command.Parameters.Add("@add", System.Data.SqlDbType.NVarChar).Value = user.Address;
                command.Parameters.Add("@mb", System.Data.SqlDbType.NVarChar).Value = user.Mobile;
                command.Parameters.Add("@em", System.Data.SqlDbType.NVarChar).Value = user.Email;
                command.Parameters.Add("@pwd", System.Data.SqlDbType.NVarChar).Value = user.Password;
                command.Parameters.Add("@cat", System.Data.SqlDbType.DateTime).Value = user.CreatedDate;
                command.Parameters.Add("@mat", System.Data.SqlDbType.DateTime).Value = user.ModifiedDate;

                command.ExecuteNonQuery();

            }   
            return true;
        }
    }
}
