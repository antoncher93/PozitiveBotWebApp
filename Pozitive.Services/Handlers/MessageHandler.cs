using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Handlers
{
    public abstract class MessageHandler
    {
        private MessageHandler _next;

        public MessageHandler SetNext(MessageHandler messageHandler)
        {
            _next = messageHandler;
            return messageHandler;
        }

        public virtual void Handle(ITelegramBotClient client, Message msg)
        {
            _next?.Handle(client, msg);
        }
    }
}
