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
    public class WantIntoChatCallbackHandler : CallbackHandler
    {
        private const string Text = "Хорошо, для того, чтобы стать участником закрытого чата 7 корпуса ЖК Позитив, необходимо подтвердить свою принадлежность. "
            + "Для этого пришлите мне фото документа, подтверждающего собственость или участие в долевом "
            + "строительстве (фото первой страницы ДДУ или т.п.)";
        private const string Text2 = "Отправьте фото первой страницы ДДУ или фото акта приемки УК";

        private readonly ApplicationContext _db;
        public WantIntoChatCallbackHandler(ApplicationContext appContext)
        {
            _db = appContext;
        }

        public override string Data => Bot.WANT_INTO_CHAT_CALLBACK_Q;

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var from = update.CallbackQuery.From;
            var user = _db.Users.FirstOrDefaultAsync(u => long.Equals(u.TelegramId, from.Id))
                .Result;
            //var user = _db.Users.FirstOrDefault(u => _check(u, from.Id));
            if (user is null)
            {
                user = new Models.User()
                {
                    Status = UserStatus.WaitingPhotoOfDoc,
                    TelegramId = from.Id,
                    ChatId = update.CallbackQuery.Message.Chat.Id,
                    FirstName = from.FirstName,
                    LastName = from.LastName,
                    UserName = from.Username
                };
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            else
            {
                user.Status = UserStatus.WaitingPhotoOfDoc;
                user.ChatId = update.CallbackQuery.Message.Chat.Id;
                user.FirstName = from.FirstName;
                user.LastName = from.LastName;
                user.UserName = from.Username;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, Text);
            client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, Text2);
        }
    }
}
