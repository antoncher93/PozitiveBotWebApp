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
    public class ReloadAdminCommandHandler : UpdateHandler
    {
        private readonly ApplicationContext _db;
        private readonly IConfiguration _configuration;

        public ReloadAdminCommandHandler(ApplicationContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var message = update.Message;

            if(message?.Entities != null 
                && message.Entities.Any() 
                && message.Entities[0].Type == MessageEntityType.BotCommand
                && message.EntityValues.ElementAt(0) == "/reload_admin")
            {
                if (long.Equals(message.From.Id, Bot.ROOT_ADMIN_ID) && message.Chat.Type == ChatType.Private)
                {
                    _configuration["AdminId"] = message.Chat.Id.ToString();
                    var mention = "[" + message.From.FirstName + "](tg://user?id=" + message.From.Id + ")";
                    var text = $"{mention} теперь главный!";
                    client.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Markdown);
                }
                else
                {
                    client.SendTextMessageAsync(message.Chat.Id, "Команда доступна только в приватном чате", ParseMode.Markdown);
                }
            }
            else base.Handle(client, update); // next handler
        }
    }
}
