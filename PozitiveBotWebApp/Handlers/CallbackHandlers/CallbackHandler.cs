using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Handlers.CallbackHandlers
{
    public abstract class CallbackHandler : UpdateHandler
    {
        public abstract string Data { get; }
        public sealed override void Handle(ITelegramBotClient client, Update update)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                && string.Equals(Data, update.CallbackQuery.Data))
            {
                Execute(client, update);
            }
            else base.Handle(client, update);

        }
        public abstract void Execute(ITelegramBotClient client, Update update);
    }
}
