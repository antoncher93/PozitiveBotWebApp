using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using PozitiveBotWebApp;
using PozitiveBotWebApp.Handlers.CallbackHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public class ApproveCallbackHandler : CallbackHandler
    {
        private readonly IAdminService _adminService;
        private readonly IRepository<Person> _persons;
        public ApproveCallbackHandler(IAdminService adminService, IRepository<Person> personRepos)
        {
            _adminService = adminService;
            _persons = personRepos;
        }

        public override string Data => Bot.APPROVE_USER;

        public override async void Execute(ITelegramBotClient client, Update update)
        {
            var from = update.CallbackQuery.From;
            var chat = update.CallbackQuery.Message.Chat;
            //var admin = _persons.FirstOrDefault(u => long.Equals(u.TelegramId, from.Id));
            if (!_adminService.IsAdmin(from, chat))
                return;

            var mention = update.CallbackQuery.Message.CaptionEntityValues.ElementAt(0);
            var id = int.Parse(mention);
            var person = _persons.GetAll().FirstOrDefault(u => Equals(u.Id, id));
            if (person is null)
                return;

            person.Status = UserStatus.Accepted;
            _persons.Update(person);

            //var text = $"Подтверждение принято. Можете вступить в закрытый чат 7 корпуса ЖК Позитив по ссылке:\n" + _configuration["ChatInviteLink"];
            //client.SendTextMessageAsync(user.ChatId, text);

            _adminService.InvitePerson(person);

            var caption = update.CallbackQuery.Message.Caption + "\nПринят!";
            await client.EditMessageCaptionAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId,
                caption, parseMode: ParseMode.Markdown);
        }
    }
}
