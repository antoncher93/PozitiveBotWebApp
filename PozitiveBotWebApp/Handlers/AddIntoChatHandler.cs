using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Handlers
{
    public class AddIntoChatHandler : UpdateHandler
    {
        public override void Handle(ITelegramBotClient client, Update update)
        {
            if(update.Type == UpdateType.CallbackQuery
                && string.Equals(update.CallbackQuery.Data, Bot.ADD_USER_INTO_CHAT))
            {
                var chat = client.GetChatAsync(Bot.TargetChatId).Result;
            }
            base.Handle(client, update);


        }
    }
}
