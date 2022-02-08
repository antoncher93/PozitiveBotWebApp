using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Pozitive.Services.Internal;

namespace Pozitive.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddPositiveServices(this IServiceCollection services)
        {
            services.AddSingleton<IAdminService, AdminService>();
            return services;
        }
    }
}
