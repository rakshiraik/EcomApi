using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomApi.Services.Models
{
    public class RequestModel
    {
        public string User { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
    }

    public class CustomerModel
    {
        public string CustomerId { get; set; }=null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? Town { get; set; }
        public string? Postcode { get; set; }
    }

    public class OrderItemModel
    {
        public string? Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceEach { get; set; }
    }

    public class OrderModel
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? DeliveryAddress { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = null!;
        public DateTime DeliveryExpected { get; set; }
    }

    public class ApiResponseModel
    {
        public CustomerModel? Customer { get; set; }
        public OrderModel? Order { get; set; }
    }

}
