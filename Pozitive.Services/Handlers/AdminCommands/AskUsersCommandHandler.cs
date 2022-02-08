using Pozitive.Entities;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp;
using PozitiveBotWebApp.Handlers.AdminCommands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Handlers.AdminCommands
{
    public class AskUsersCommandHandler : AdminCommandHandler
    {
        private readonly IRepository<Person> _persons;

        public AskUsersCommandHandler(IRepository<Person> persons)
        {
            _persons = persons;
        }
        public override string Name { get; } = "/ask_users_1";

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var button = InlineKeyboardButton.WithCallbackData("Да", Bot.EXIST_IN_CHAT_REPLY);
            var button2 = InlineKeyboardButton.WithCallbackData("Нет", Bot.DONT_EXIST_IN_CHAT_REPLY);
            var keyboard = new InlineKeyboardMarkup(new[] { button, button2 });

            foreach (var person in _persons)
            {
                //client.SendTextMessageAsync(user.ChatId, "Получилось ли попасть в закрытый чат 7 корпуса ЖК Позитив?", replyMarkup: keyboard);
            }
        }
    }
}
