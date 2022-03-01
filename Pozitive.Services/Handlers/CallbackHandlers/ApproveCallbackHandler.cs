using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public class ApproveCallbackHandler : CallbackHandler
    {
        private readonly IAdminService _adminService;

        public ApproveCallbackHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public override string Data => Bot.APPROVE_USER;

        public override void Handle(ITelegramBotClient client, Update update)
        {
            if(!string.Equals(update.CallbackQuery.Data, Bot.APPROVE_USER))
            {
                base.Handle(client, update);
                return;
            }

            var from = update.CallbackQuery.From;
            var msg = update.CallbackQuery.Message;

            if (!_adminService.IsAdmin(from.Id))
                return;

            var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
            var id = int.Parse(mention);
            var photo = msg.Photo;
            _adminService.InvitePerson(id, msg.Photo[msg.Photo.Length - 1]);

            //var caption = update.CallbackQuery.Message.Caption + "\nПринят!";
            //client.EditMessageCaptionAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
            //    caption, parseMode: ParseMode.Markdown);


        }
    }
}
