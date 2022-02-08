using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pozitive.Services.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Handlers.AdminCommands
{
    public abstract class AdminCommandHandler : UpdateHandler
    {
        public abstract string Name { get; }
        public abstract void Execute(ITelegramBotClient client, Update update);

        public override void Handle(ITelegramBotClient client, Update update)
        {
            if(!_Handle(client, update))
                base.Handle(client, update);
        }
        private bool _Handle(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;
            if (msg is null)
                return false;

            var from = update.Message.From;
            if (!Equals(from.Id, Bot.ROOT_ADMIN_ID))
                return false;

            for(int i = 0; i< msg.Entities.Length; i++)
            {
                var entity = msg.Entities[i];
                if(entity.Type == MessageEntityType.BotCommand)
                {
                    var command = msg.EntityValues.ElementAt(i);
                    if(string.Equals(command, Name))
                    {
                        Execute(client, update);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
