using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Handlers
{
    public class StartUpdateHandler : UpdateHandler
    {
        private readonly IRepository<Person> _persons;
        private readonly IAdminService _adminService;
        private const string TEXT = "Привет. Это чат-бот 7 корпуса ЖК Позитив.";
        private const string TEXT2 = "Желаете вступить в закрытый чат";
        public StartUpdateHandler(IAdminService adminService, IRepository<Person> persons)
        {
            _persons = persons;
            _adminService = adminService;
        }
        
        public override async void Handle(ITelegramBotClient client, Update update)
        {
            var entity = update.Message?.Entities?.FirstOrDefault();
            if(entity is { Type: MessageEntityType.BotCommand } 
               && string.Equals(update.Message.EntityValues?.First(), "/start"))
            {
                var from = update.Message.From;
                var person = _persons.GetAll().FirstOrDefault(u => Equals(u.TelegramId, @from.Id));
                if (person is null)
                {
                    person = new Person()
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

                person.DialogStatus = null;
                _persons.Update(person);

                await client.SendTextMessageAsync(update.Message.Chat.Id, TEXT, replyToMessageId: update.Message.MessageId);

                if(!_adminService.IsChatMember(person.TelegramId))
                {
                    var button = InlineKeyboardButton.WithCallbackData("Да, хочу в чат!", Bot.WANT_INTO_CHAT_CALLBACK_Q);
                    var keyboard = new InlineKeyboardMarkup(new[] { button });
                    await client.SendTextMessageAsync(update.Message.Chat.Id, TEXT2, ParseMode.Markdown, replyMarkup: keyboard);
                }
            }
            else base.Handle(client, update);
        }
    }
}
