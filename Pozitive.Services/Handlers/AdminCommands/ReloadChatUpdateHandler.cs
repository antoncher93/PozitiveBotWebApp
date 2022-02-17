using Pozitive.Entities;
using Pozitive.Entities.Repos;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers.AdminCommands
{
    public class ReloadChatUpdateHandler : AdminCommandHandler
    {
        private readonly IRepository<Person> _persons;
        public ReloadChatUpdateHandler(IAdminService adminService, IRepository<Person> persons) 
            : base(adminService, persons)
        {
            _persons = persons;
        }

        public override string CommandName { get; } = "/reload_chat";

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            if(message.Chat.Type == ChatType.Private)
            {
                var person = _persons.GetAll()
                    .FirstOrDefault(p => long.Equals(p.TelegramId, message.From.Id));

                if(person != null)
                {
                    client.SendTextMessageAsync(message.Chat.Id, "Перешлите мне сообщение из чата");
                    person.DialogStatus = "reload_chat_message";
                    _persons.Update(person);
                }
            }
            else
            {
                _adminService.ReloadChat(message.Chat.Id);
            }
            
        }
    }
}
