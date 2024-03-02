using EcomApi.Services.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace EcomApi.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _dbConnection;

        public OrderService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultDb"));
        }

        private  void CloseConnection(IDbConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public  void OpenConnection(IDbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public ApiResponseModel GetMostRecentOrder(string email, string customerId)
        {
            string query = @"
        SELECT TOP 1
            c.FIRSTNAME,
            c.LASTNAME,
            o.ORDERID AS OrderNumber,
            FORMAT(o.ORDERDATE, 'dd-MMM-yyyy') AS ORDERDATE,
            CONCAT(c.HOUSENO, ' ', c.STREET, ', ', c.TOWN, ', ', c.POSTCODE) AS DeliveryAddress,
            CASE
                WHEN o.CONTAINSGIFT = 1 THEN 'Gift'
                ELSE p.PRODUCTNAME
            END AS Product,
            oi.QUANTITY,
            oi.PRICE AS PriceEach,
            FORMAT(o.DELIVERYEXPECTED, 'dd-MMM-yyyy') AS DELIVERYEXPECTED
        FROM
            CUSTOMERS c
            LEFT JOIN ORDERS o ON c.CUSTOMERID = o.CUSTOMERID
            LEFT JOIN ORDERITEMS oi ON o.ORDERID = oi.ORDERID
            LEFT JOIN PRODUCTS p ON oi.PRODUCTID = p.PRODUCTID
        WHERE
            c.EMAIL = @Email
            AND c.CUSTOMERID = @CustomerId
        ORDER BY
            o.ORDERDATE DESC";

            using (var command = new SqlCommand(query, _dbConnection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                OpenConnection(_dbConnection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var apiResponse = new ApiResponseModel
                        {
                            Customer = new CustomerModel
                            {
                                FirstName = reader["FIRSTNAME"] == DBNull.Value ? null : reader["FIRSTNAME"].ToString(),
                                LastName = reader["LASTNAME"] == DBNull.Value ? null : reader["LASTNAME"].ToString()
                            },
                            Order = new OrderModel
                            {
                                OrderNumber = reader["OrderNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderNumber"]),
                                OrderDate = reader["ORDERDATE"] == DBNull.Value ? DateTime.MinValue : DateTime.ParseExact(reader["ORDERDATE"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                                DeliveryAddress = reader["DeliveryAddress"] == DBNull.Value ? null : reader["DeliveryAddress"].ToString(),
                                OrderItems = new List<OrderItemModel>
                        {
                            new OrderItemModel
                            {
                                Product = reader["Product"] == DBNull.Value ? null : reader["Product"].ToString(),
                                Quantity = reader["QUANTITY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QUANTITY"]),
                                PriceEach = reader["PriceEach"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PriceEach"])
                            }
                        },
                                DeliveryExpected = reader["DELIVERYEXPECTED"] == DBNull.Value ? DateTime.MinValue : DateTime.ParseExact(reader["DELIVERYEXPECTED"].ToString(), "dd-MMM-yyyy", CultureInfo.InvariantCulture)
                            }
                        };

                        return apiResponse;
                    }
                }
                CloseConnection(_dbConnection);

            }

            return null!;
        }

        public CustomerModel GetCustomerById(string customerId)
        {
            string query = @"
            SELECT
                CUSTOMERID,
                FIRSTNAME,
                LASTNAME,
                EMAIL,
                HOUSENO,
                STREET,
                TOWN,
                POSTCODE
            FROM
                CUSTOMERS
            WHERE
                CUSTOMERID = @CustomerId";

            using (var command = new SqlCommand(query, _dbConnection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);

                OpenConnection(_dbConnection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var customer = new CustomerModel
                        {
                            CustomerId = reader["CUSTOMERID"].ToString()!,
                            FirstName = reader["FIRSTNAME"].ToString(),
                            LastName = reader["LASTNAME"].ToString(),
                            Email = reader["EMAIL"].ToString(),
                            HouseNo = reader["HOUSENO"].ToString(),
                            Street = reader["STREET"].ToString(),
                            Town = reader["TOWN"].ToString(),
                            Postcode = reader["POSTCODE"].ToString()
                        };
                       
                        return customer;
                    }
                }

                CloseConnection(_dbConnection);
            }

            return null; 
        }

    }
}
