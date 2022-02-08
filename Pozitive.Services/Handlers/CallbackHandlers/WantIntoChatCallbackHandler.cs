using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public class WantIntoChatCallbackHandler : CallbackHandler
    {
        private readonly IRepository<Person> _persons;

        private const string Text = "Хорошо, для того, чтобы стать участником закрытого чата 7 корпуса ЖК Позитив, необходимо подтвердить свою принадлежность. "
                                    + "Для этого пришлите мне фото документа, подтверждающего собственость или участие в долевом "
                                    + "строительстве (фото первой страницы ДДУ или т.п.)";
        private const string Text2 = "Отправьте фото первой страницы ДДУ или фото акта приемки УК";

        public override string Data => Bot.WANT_INTO_CHAT_CALLBACK_Q;

        public WantIntoChatCallbackHandler(IRepository<Person> persons)
        {
            _persons = persons; 
        }

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var from = update.CallbackQuery.From;
            var person = _persons.FirstOrDefault(u => long.Equals(u.TelegramId, from.Id));
            if (person is null)
            {
                person = new Person()
                {
                    Status = UserStatus.WaitingPhotoOfDoc,
                    TelegramId = from.Id,
                    ChatId = update.CallbackQuery.Message.Chat.Id,
                    FirstName = from.FirstName,
                    LastName = from.LastName,
                    UserName = from.Username
                };
                _persons.Add(person);
            }
            else
            {
                person.Status = UserStatus.WaitingPhotoOfDoc;
                person.ChatId = update.CallbackQuery.Message.Chat.Id;
                person.FirstName = from.FirstName;
                person.LastName = from.LastName;
                person.UserName = from.Username;
                _persons.Update(person);
            }
            client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, Text);
            client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, Text2);
        }
    }
}
