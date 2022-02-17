using Pozitive.Entities;
using Pozitive.Entities.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;

namespace Pozitive.Services.Handlers
{
    class MessageToReloadChatHandler : UpdateHandler
    {
        private readonly IRepository<Person> _persons;

        public MessageToReloadChatHandler(IRepository<Person> persons)
        {
            _persons = persons;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;
            if(msg != null)
            {
                var person = _persons.GetAll()
                    .FirstOrDefault(p => long.Equals(p.TelegramId, msg.From.Id));
                if(person != null)
                {
                    if(person.DialogStatus == "reload_chat_message")
                    {
                        var d = msg.ForwardFromChat;
                        //person.DialogStatus = null;
                        //_persons.Update(person);
                    }
                }
            }

            base.Handle(client, update);
        }
    }
}
