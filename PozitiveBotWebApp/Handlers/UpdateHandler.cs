using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Handlers
{
    public class UpdateHandler
    {
        private UpdateHandler _next;

        public UpdateHandler SetNext(UpdateHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual void Handle(ITelegramBotClient client, Update update)
        {
            _next?.Handle(client, update);
        }
    }
}
