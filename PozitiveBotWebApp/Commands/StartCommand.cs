using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Commands
{
    public class StartCommand : BaseCommand
    {
        public override string Name { get; } = "/start";

        public override void Execute(ITelegramBotClient client, Message msg)
        {
            client.SendTextMessageAsync(msg.Chat.Id, "Стартуем!");
        }
    }
}
