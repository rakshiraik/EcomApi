using EcomApi.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomApi.Services.Services
{
    public interface IOrderService
    {
        public ApiResponseModel GetMostRecentOrder(string email, string customerId);

        public CustomerModel GetCustomerById(string customerId);
    }
}
