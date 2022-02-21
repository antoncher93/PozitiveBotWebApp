using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Handlers.AdminCommands
{
    public class AskUsersCommandHandler : AdminCommandHandler
    {
        private readonly IRepository<Person> _persons;

        public AskUsersCommandHandler(IAdminService adminService, IRepository<Person> persons)
            : base(adminService, persons)
        {
            _persons = persons;
        }

        public override string CommandName { get; } = "/ask_users_1";

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var button = InlineKeyboardButton.WithCallbackData("Да", Bot.EXIST_IN_CHAT_REPLY);
            var button2 = InlineKeyboardButton.WithCallbackData("Нет", Bot.DONT_EXIST_IN_CHAT_REPLY);
            var keyboard = new InlineKeyboardMarkup(new[] { button, button2 });

            foreach (var person in _persons.GetAll())
            {
                if(!_adminService.IsChatMember(person.TelegramId))
                    client.SendTextMessageAsync(person.ChatId, "Получилось ли попасть в закрытый чат 7 корпуса ЖК Позитив?", replyMarkup: keyboard);
            }
        }
    }
}
