using System.Linq;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers
{
    public class ReloadAdminCommandHandler : UpdateHandler
    {
        public ReloadAdminCommandHandler()
        {
            
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            base.Handle(client, update);

            //var message = update.Message;

            //if(message?.Entities != null 
            //    && message.Entities.Any() 
            //    && message.Entities[0].Type == MessageEntityType.BotCommand
            //    && message.EntityValues.ElementAt(0) == "/reload_admin")
            //{
            //    if (long.Equals(message.From.Id, Bot.ROOT_ADMIN_ID) && message.Chat.Type == ChatType.Private)
            //    {
            //        _configuration["AdminId"] = message.Chat.Id.ToString();
            //        var mention = "[" + message.From.FirstName + "](tg://user?id=" + message.From.Id + ")";
            //        var text = $"{mention} теперь главный!";
            //        client.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Markdown);
            //    }
            //    else
            //    {
            //        client.SendTextMessageAsync(message.Chat.Id, "Команда доступна только в приватном чате", ParseMode.Markdown);
            //    }
            //}
            //else base.Handle(client, update); // next handler
        }
    }
}
