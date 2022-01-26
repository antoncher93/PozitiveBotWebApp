using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PozitiveBotWebApp
{
    interface ITelegramBot
    {
        Task<TelegramBotClient> GetAsync();
    }
}
