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
using Telegram.Bot.Types.ReplyMarkups;

namespace PozitiveBotWebApp.Handlers
{
    public class PhotoHandler : UpdateHandler
    {
        private readonly ApplicationContext _db;
        private readonly IConfiguration _configuration;
        public PhotoHandler(ApplicationContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var message = update.Message;

            if (message != null)
            {
                var user = _db.Users.FirstOrDefaultAsync(u => long.Equals(u.TelegramId, message.From.Id)).Result;
                if(user!= null && user.Status == UserStatus.WaitingPhotoOfDoc)
                {
                    if (message.Photo != null && message.Photo.Any())
                    {
                        if (int.TryParse(_configuration["AdminId"], out var chatId))
                        {
                            var photo = message.Photo[0];
                            var buttonYes = InlineKeyboardButton.WithCallbackData("Принять", Bot.APPROVE_USER);
                            var buttonNo = InlineKeyboardButton.WithCallbackData("Отклонить", Bot.REJECT_USER);
                            var keyboard = new InlineKeyboardMarkup(new[] { buttonYes, buttonNo });

                            var mention = "[" + user.Id + "](tg://user?id=" + message.From.Id + ")";
                            var text = $"Пользователь {mention} хочет в чат";
                            client.SendPhotoAsync(chatId, photo.FileId, text, ParseMode.Markdown, replyMarkup: keyboard);
                        }
                    }
                    else
                    {
                        client.SendTextMessageAsync(message.Chat.Id, "В сообщении отсутствуют фото необходимых документов");
                    }
                }
            }
            else base.Handle(client, update);
        }
    }
}
