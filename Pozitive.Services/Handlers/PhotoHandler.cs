using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Handlers
{
    public class PhotoHandler : UpdateHandler
    {
        private readonly IRepository<Person> _users;
        private readonly IAdminService _adminService;
        public PhotoHandler(IRepository<Person> users, IAdminService adminService)
        {
            _users = users;
            _adminService = adminService;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var message = update.Message;

            if (message != null)
            {
                var user = _users.FirstOrDefault(u => long.Equals(u.TelegramId, message.From.Id));
                if(user!= null && user.Status == UserStatus.WaitingPhotoOfDoc)
                {
                    if (message.Photo != null && message.Photo.Any())
                    {
                        var photo = message.Photo[0];
                        var person = _users.FirstOrDefault(p => long.Equals(p.TelegramId, message.From.Id));
                        _adminService.ForwardDocumentToAdmin(person, photo.FileId);

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
