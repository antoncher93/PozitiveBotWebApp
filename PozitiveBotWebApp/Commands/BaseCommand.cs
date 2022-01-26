using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract void Execute(ITelegramBotClient client, Message msg);
        public async Task ExecuteAsync(ITelegramBotClient client, Message msg)
        {
            await Task.Factory.StartNew(() => Execute(client, msg));
        }
    }
}
