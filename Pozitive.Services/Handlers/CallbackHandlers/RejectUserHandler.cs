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
    public class RejectUserHandler : CallbackHandler
    {
        private readonly IAdminService _adminService;
        private readonly IRepository<Person> _persons;

        
        public override string Data { get; } = Bot.REJECT_USER;

        public RejectUserHandler(IAdminService adminService, IRepository<Person> persons)
        {
            _adminService = adminService;
            _persons = persons;
        }

        
        public override void Handle(ITelegramBotClient client, Update update)
        {
            if (string.Equals(update.CallbackQuery.Data, Bot.REJECT_USER))
            {
                var admin = update.CallbackQuery.From;
                var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
                var personId = int.Parse(mention);
                var person = _persons.GetAll()
                    .FirstOrDefault(p => Equals(personId, p.Id));
                if (person is null)
                    return;

                _adminService.DeclinePerson(admin, person);

                var caption = update.CallbackQuery.Message.Caption + "\nОТКЛОНЕН!";
                client.EditMessageCaptionAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                    caption, parseMode: ParseMode.Markdown);
            }
            else base.Handle(client, update);
          
        }
    }
}
