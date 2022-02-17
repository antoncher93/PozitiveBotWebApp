using Pozitive.Entities;
using Pozitive.Entities.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers.AdminCommands
{
    class SendToAllAdminCommand : AdminCommandHandler
    {
        private readonly IRepository<Person> _persons;
        public SendToAllAdminCommand(IAdminService adminService, IRepository<Person> persons) : base(adminService, persons)
        {
            _persons = persons;
        }

        public override string CommandName { get; } = "/send_to_all";

        public override void Execute(ITelegramBotClient client, Update update)
        {
            var lines = update.Message.Text.Split("\n");
            if(lines.Length > 1)
            {
                var sb = new StringBuilder();
                for (int i = 1; i < lines.Length; i++)
                    sb.AppendLine(lines[i]);

                var text = sb.ToString();
                int j = 0;
                foreach(var person in _persons.GetAll())
                {
                    if(person.ChatId != null)
                    {
                        var chatId = person.ChatId.Value;
                        client.SendTextMessageAsync(chatId, text);
                        j++;
                    }
                }
                client.SendTextMessageAsync(update.Message.From.Id, $"Отправлено {j} пользователям");
            }
        }
    }
}
