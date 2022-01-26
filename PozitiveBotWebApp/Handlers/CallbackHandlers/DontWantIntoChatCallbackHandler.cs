using Microsoft.EntityFrameworkCore;
using PozitiveBotWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Handlers.CallbackHandlers
{
    public class DontWantIntoChatCallbackHandler : CallbackHandler
    {
        public override string Data => Bot.DONT_WANT_INTO_CHAT_CALLBACK_Q;

        private readonly ApplicationContext _db;

        public DontWantIntoChatCallbackHandler(ApplicationContext db)
        {
            _db = db;
        }

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var msg = update.CallbackQuery.Message;
            var from = update.CallbackQuery.From;

            var user = _db.Users.FirstOrDefault(u => Equals(from.Id, u.TelegramId));

            if(user != null)
            {
                user.Status = UserStatus.Unknown;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChangesAsync();
            }

            client.SendTextMessageAsync(msg.Chat.Id, "Хорошо, больше не буду беспокоить");
        }
    }
}
