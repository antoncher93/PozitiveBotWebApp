using PozitiveBotWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PozitiveBotWebApp.Handlers.AdminCommands
{
    public class AskUsersCommandHandler : AdminCommandHandler
    {
        private readonly ApplicationContext _db;
        public override string Name { get; } = "/ask_users_1";

        public AskUsersCommandHandler(ApplicationContext db)
        {
            _db = db;
        }


        public override void Execute(ITelegramBotClient client, Update update)
        {
            var button = InlineKeyboardButton.WithCallbackData("Да", Bot.EXIST_IN_CHAT_REPLY);
            var button2 = InlineKeyboardButton.WithCallbackData("Нет", Bot.DONT_EXIST_IN_CHAT_REPLY);
            var keyboard = new InlineKeyboardMarkup(new[] { button, button2 });

            foreach (var user in _db.Users)
            {
                client.SendTextMessageAsync(user.ChatId, "Получилось ли попасть в закрытый чат 7 корпуса ЖК Позитив?", replyMarkup: keyboard);
            }
        }
    }
}
