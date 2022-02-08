using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp;
using PozitiveBotWebApp.Handlers.CallbackHandlers;
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

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var admin = update.CallbackQuery.From;
            var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
            var personId = int.Parse(mention);
            var person = _persons.FirstOrDefault(p => Equals(personId, p.Id));
            if(person is null)
                return;

            _adminService.DeclinePerson(admin, person.TelegramId);
        }

        /*
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
        }*/
    }
}
