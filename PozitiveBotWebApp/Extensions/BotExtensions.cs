using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Extensions
{
    public static class BotExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            var client = new TelegramBotClient(configuration["Token"]);
            var url = configuration["Url"];
            var hook = string.Format(url, "api/message/update");

            client.SetWebhookAsync(hook, allowedUpdates: new[] { UpdateType.Message, UpdateType.CallbackQuery });
            return serviceDescriptors
                .AddSingleton<ITelegramBotClient>(p => client);
        }
    }
}
