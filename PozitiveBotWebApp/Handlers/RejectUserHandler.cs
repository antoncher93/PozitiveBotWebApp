using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PozitiveBotWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PozitiveBotWebApp.Handlers
{
    public class RejectUserHandler : UpdateHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _db;

        public RejectUserHandler(IConfiguration configuration, ApplicationContext db)
        {
            _configuration = configuration;
            _db = db;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            if (!_Handle(client, update))
                base.Handle(client, update);
        }

        private bool _Handle(ITelegramBotClient client, Update update)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                var adminChatId = long.Parse(_configuration["AdminId"]);
                if (string.Equals(update.CallbackQuery.Data, Bot.REJECT_USER)
                    && long.Equals(update.CallbackQuery.From.Id, adminChatId))
                {
                    var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
                    var id = int.Parse(mention);
                    var user = _db.Users.FirstOrDefault(u => Equals(u.Id, id));
                    user.Status = UserStatus.Waiting;
                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChangesAsync();

                    var text = $"Подтверждение не принято. Попробуйте прикрепить другое фото документа";
                    client.SendTextMessageAsync(user.ChatId, text);

                    var caption = update.CallbackQuery.Message.Caption + "\n<b>Отклонен!</b>";
                    client.EditMessageCaptionAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                        caption, parseMode: ParseMode.Html);
                    return true;
                }
            }
            return false;
        }
    }
}
