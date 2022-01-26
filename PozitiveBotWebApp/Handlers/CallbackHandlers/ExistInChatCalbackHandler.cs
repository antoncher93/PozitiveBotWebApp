using Microsoft.EntityFrameworkCore;
using PozitiveBotWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Handlers.CallbackHandlers
{
    public class ExistInChatCalbackHandler : CallbackHandler
    {
        private readonly ApplicationContext _db;
        public ExistInChatCalbackHandler(ApplicationContext db)
        {
            _db = db;
        }

        public override string Data => Bot.EXIST_IN_CHAT_REPLY;

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var msg = update.CallbackQuery.Message;
            var from = update.CallbackQuery.From;
            var user = _db.Users.FirstOrDefault(u => Equals(u.TelegramId, from.Id));

            if (user is null)
                return;

            var member = client.GetChatMemberAsync(update.CallbackQuery.Message.Chat.Id, user.TelegramId).Result;
            if (member is null)
            {
                user.Status = UserStatus.Unknown;
                client.SendTextMessageAsync(msg.Chat.Id, "Странно, не нашел Вас в чате.");
            }
            else
            {
                user.Status = UserStatus.ExistInChat;
            }

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChangesAsync();
        }
    }
}
