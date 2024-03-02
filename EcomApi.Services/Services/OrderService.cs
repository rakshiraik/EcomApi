using EcomApi.Services.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomApi.Services.Services
{
    public class OrderService:IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _dbConnection;


        public OrderService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultDb"));
        }

        public ApiResponseModel GetMostRecentOrder(string email, string customerId)
        {
            string query = @"
        SELECT TOP 1
            c.FIRSTNAME,
            c.LASTNAME,
            o.ORDERID AS OrderNumber,
            o.ORDERDATE,
            CONCAT(c.HOUSENO, ' ', c.STREET, ', ', c.TOWN, ', ', c.POSTCODE) AS DeliveryAddress,
            oi.PRODUCTID AS Product,
            oi.QUANTITY,
            oi.PRICE AS PriceEach,
            o.DELIVERYEXPECTED
        FROM
            CUSTOMERS c
            INNER JOIN ORDERS o ON c.CUSTOMERID = o.CUSTOMERID
            INNER JOIN ORDERITEMS oi ON o.ORDERID = oi.ORDERID
        WHERE
            c.EMAIL = @Email
            AND c.CUSTOMERID = @CustomerId
        ORDER BY
            o.ORDERDATE DESC";

            using (var command = new SqlCommand(query, _dbConnection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@CustomerId", customerId);

                _dbConnection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var apiResponse = new ApiResponseModel
                        {
                            Customer = new CustomerModel
                            {
                                FirstName = reader["FIRSTNAME"].ToString()!,
                                LastName = reader["LASTNAME"].ToString()!
                            },
                            Order = new OrderModel
                            {
                                OrderNumber = Convert.ToInt32(reader["OrderNumber"]),
                                OrderDate = Convert.ToDateTime(reader["ORDERDATE"]),
                                DeliveryAddress = reader["DeliveryAddress"].ToString()!,
                                OrderItems = new List<OrderItemModel>
                        {
                            new OrderItemModel
                            {
                                Product = reader["Product"].ToString()!,
                                Quantity = Convert.ToInt32(reader["QUANTITY"]),
                                PriceEach = Convert.ToDecimal(reader["PriceEach"])
                            }
                        },
                                DeliveryExpected = Convert.ToDateTime(reader["DELIVERYEXPECTED"])
                            }
                        };

                        return apiResponse;
                    }
                }
            }

            return null!;
        }

    }

}
