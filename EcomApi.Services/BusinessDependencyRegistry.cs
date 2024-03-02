using EcomApi.Helper;
using EcomApi.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomApi.Services
{
    public static class BusinessDependencyRegistry
    {
        public static void RegisterDependency(this IServiceCollection services)
        {
            services.AddTransient<DatabaseHelper>();
            services.AddTransient<IOrderService, OrderService>();
        }
    }
}
