using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers
{
    public class PhotoHandler : PrivateChatHandler
    {
        private readonly IAdminService _adminService;
        public PhotoHandler(IAdminService adminService, IRepository<Person> persons) : base(persons)
        {
            _adminService = adminService;
        }

        public override void HandleMessage(ITelegramBotClient client, Person person, Message message)
        {
            if (message != null && person.Status == UserStatus.WaitingPhotoOfDoc)
            {
                if (message.Photo != null && message.Photo.Any())
                {
                    var photo = message.Photo[0];
                    _adminService.ForwardDocumentToAdmin(person, photo.FileId);
                }
                else
                {
                    client.SendTextMessageAsync(message.Chat.Id, "В сообщении отсутствуют фото необходимых документов");
                }
            }
        }
    }
}
