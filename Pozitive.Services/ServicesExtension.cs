using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Pozitive.Services.Internal;
using Telegram.Bot;

namespace Pozitive.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddPositiveBotServices(this IServiceCollection services, string token)
        {
            services.AddSingleton<ITelegramBotClient>(s => new TelegramBotClient(token));
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IBot, Bot>();
            return services;
        }
    }
}
