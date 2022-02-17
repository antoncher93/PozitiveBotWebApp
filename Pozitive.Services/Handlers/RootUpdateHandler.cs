using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers
{
    class RootUpdateHandler : UpdateHandler
    {
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;
            if (msg != null && msg.Chat.Type == ChatType.Private)
            {
                base.Handle(client, update);
            }
        }
    }
}
