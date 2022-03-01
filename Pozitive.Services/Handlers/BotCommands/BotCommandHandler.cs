using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using Telegram.Bot.Types.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Entities;

namespace Pozitive.Services.Handlers.BotCommands
{
    public abstract class BotCommandHandler
    {
        private BotCommandHandler _next;

        protected abstract string Name { get; }
        protected abstract void Execute(ITelegramBotClient client, Update update);

        public BotCommandHandler SetNext(BotCommandHandler next)
        {
            _next = next;
            return next;
        }

        public void Handle(ITelegramBotClient client, Update update)
        {
            if(update.Type == UpdateType.Message)
            {
                var msg = update.Message;
                if (msg.Entities != null)
                {
                    for(int i = 0; i < msg.Entities.Length; i++)
                    {
                        if(msg.Entities[i].Type == MessageEntityType.BotCommand)
                        {
                            var command = msg.EntityValues.ElementAt(i);
                            if (string.Equals(command, Name))
                            {
                                Execute(client, update);
                                return;
                            }
                        }
                    }
                }
            }

            _next.Handle(client, update);
        }

        public static BotCommandHandler Create(IAdminService adminService, IRepository<Person> persons, IRepository<Entities.Document> documents)
        {
            var tail = new StartCommandHandler(adminService, persons);
            tail.SetNext(new ReloadChatUpdateHandler(adminService))
                .SetNext(new LoadDocumentCommandHandler(adminService, documents));
            return tail;
        }
    }
}
