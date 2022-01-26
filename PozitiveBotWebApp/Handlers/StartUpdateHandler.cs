using Microsoft.EntityFrameworkCore;
using PozitiveBotWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PozitiveBotWebApp.Handlers
{
    public class StartUpdateHandler : UpdateHandler
    {
        
        private readonly ApplicationContext _db;
        public StartUpdateHandler(ApplicationContext appContext)
        {
            _db = appContext;
        }
        
        public override async void Handle(ITelegramBotClient client, Update update)
        {
            var entity = update.Message?.Entities?.FirstOrDefault();

           
           
            if(entity != null 
                && entity.Type == MessageEntityType.BotCommand 
                && string.Equals(update.Message.EntityValues?.First(), "/start"))
            {
                var from = update.Message.From;
                var user = _db.Users.FirstOrDefault(u => Equals(u.TelegramId, ));
                if(user is null)
                {
                    user = new Models.User()
                    {
                        TelegramId = from.Id,
                        FirstName = from.FirstName,
                        Status = UserStatus.Unknown,
                        LastName = from.LastName,
                        ChatId = update.Message.Chat.Id, 
                        UserName = from.Username
                    };
                }
                await Bot.AskUserWantIntoChatAsync(client, update.Message.Chat.Id);
            }
            else base.Handle(client, update);

        }
    }
}
