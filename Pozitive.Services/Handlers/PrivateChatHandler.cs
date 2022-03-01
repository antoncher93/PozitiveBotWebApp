using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;

namespace Pozitive.Services.Handlers
{
    public abstract class PrivateChatHandler : UpdateHandler
    {
        private readonly IRepository<Person> _persons;

        public PrivateChatHandler(IRepository<Person> persons)
        {
            _persons = persons;
        }

        public sealed override void Handle(ITelegramBotClient client, Update update)
        {
            bool handled = false;
            if (update.Type == UpdateType.Message && update.Message.Chat.Type == ChatType.Private)
            {
                var message = update.Message;
                
                if (message != null)
                {
                    var person = _persons.GetAll().FirstOrDefault(p => long.Equals(p.TelegramId, message.From.Id));
                    if (person.Status == UserStatus.WaitingPhotoOfDoc)
                    {
                        HandleMessage(client, person, message);
                        handled = true;
                    }
                }
            }

            if(!handled)
                base.Handle(client, update);
        }

        public abstract void HandleMessage(ITelegramBotClient client, Person person, Message msg);
    }
}
