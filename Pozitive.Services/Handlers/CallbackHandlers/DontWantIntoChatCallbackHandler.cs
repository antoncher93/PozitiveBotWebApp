using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using PozitiveBotWebApp;
using PozitiveBotWebApp.Handlers.CallbackHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public class DontWantIntoChatCallbackHandler : CallbackHandler
    {
        private readonly IRepository<Person> _persons;
        public override string Data => Bot.DONT_WANT_INTO_CHAT_CALLBACK_Q;

        public DontWantIntoChatCallbackHandler(IRepository<Person> persons)
        {
            _persons = persons; 
        }

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var msg = update.CallbackQuery.Message;
            var from = update.CallbackQuery.From;
            var person = _persons.FirstOrDefault(u => Equals(from.Id, u.TelegramId));

            if(person != null)
            {
                person.Status = UserStatus.Unknown;
                _persons.Update(person);
            }

            client.SendTextMessageAsync(msg.Chat.Id, "Хорошо, больше не буду беспокоить");
        }
    }
}
