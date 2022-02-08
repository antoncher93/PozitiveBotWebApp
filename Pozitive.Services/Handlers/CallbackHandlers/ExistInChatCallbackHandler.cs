using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers.CallbackHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Handlers.CallbackHandlers
{
    public class ExistInChatCallbackHandler : CallbackHandler
    {
        private readonly IRepository<Person> _persons;

        public ExistInChatCallbackHandler(IRepository<Person> persons)
        {
            _persons = persons;
        }

        public override string Data => Bot.EXIST_IN_CHAT_REPLY;

        public override async void Execute(ITelegramBotClient client, Update update)
        {
            var from = update.CallbackQuery.From;
            var person = _persons.FirstOrDefault(u => Equals(u.TelegramId, from.Id));

            if (person is null)
                return;

            person.Status = UserStatus.ExistInChat;
            _persons.Update(person);
        }
    }
}
