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

namespace PozitiveBotWebApp.Handlers.CallbackHandlers
{
    public class ApproveCalbackHandler : CallbackHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _db;

        public ApproveCalbackHandler(IConfiguration configuration, ApplicationContext db)
        {
            _configuration = configuration;
            _db = db;
        }

        public override string Data => Bot.APPROVE_USER;

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var adminChatId = long.Parse(_configuration["AdminId"]);
            if (long.Equals(update.CallbackQuery.From.Id, Bot.ROOT_ADMIN_ID))
            {
                var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
                var id = int.Parse(mention);
                var user = _db.Users.FirstOrDefault(u => Equals(u.Id, id));
                if (user is null)
                    return;

                user.Status = UserStatus.Accepted;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChangesAsync();

                var text = $"Подтверждение принято. Можете вступить в закрытый чат 7 корпуса ЖК Позитив по ссылке:\n" + _configuration["ChatInviteLink"];
                client.SendTextMessageAsync(user.ChatId, text);

                var caption = update.CallbackQuery.Message.Caption + "\nПринят!";
                client.EditMessageCaptionAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                    caption, parseMode: ParseMode.Markdown);
            }
        }
    }
}
