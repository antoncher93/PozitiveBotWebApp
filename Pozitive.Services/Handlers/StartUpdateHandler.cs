﻿using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers
{
    public class StartUpdateHandler : UpdateHandler
    {
        private readonly IRepository<Person> _persons;
        public StartUpdateHandler(IRepository<Person> persons)
        {
            _persons = persons;
        }
        
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var entity = update.Message?.Entities?.FirstOrDefault();
            if(entity is { Type: MessageEntityType.BotCommand } 
               && string.Equals(update.Message.EntityValues?.First(), "/start"))
            {
                var from = update.Message.From;
                var person = _persons.FirstOrDefault(u => Equals(u.TelegramId, @from.Id));
                if (person is null)
                {
                    person = new Pozitive.Entities.Person()
                    {
                        TelegramId = @from.Id,
                        FirstName = @from.FirstName,
                        Status = UserStatus.Unknown,
                        LastName = @from.LastName,
                        ChatId = update.Message.Chat.Id,
                        UserName = @from.Username
                    };

                    _persons.Add(person);
                }
            }
            else base.Handle(client, update);
        }
    }
}