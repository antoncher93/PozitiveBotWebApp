using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services;
using Pozitive.Services.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers.AdminCommands
{
    public abstract class AdminCommandHandler : UpdateHandler
    {
        protected readonly IAdminService _adminService;
        private readonly IRepository<Person> _persons;
        public abstract string CommandName { get; }
        public abstract void Execute(ITelegramBotClient client, Update update);

        public AdminCommandHandler(IAdminService adminService, IRepository<Person> persons)
        {
            _adminService = adminService;
            _persons = persons;
        }

        public sealed override void Handle(ITelegramBotClient client, Update update)
        {
            if(!_Handle(client, update))
                base.Handle(client, update);
        }
        private bool _Handle(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;
            if (msg?.Entities is null)
                return false;

            var person = _persons.GetAll()
                .FirstOrDefault(p => long.Equals(p.TelegramId, msg.From.Id));

            if (_adminService.IsAdmin(msg.From.Id))
                return false;

            for (int i = 0; i< msg.Entities.Length; i++)
            {
                var entity = msg.Entities[i];
                if(entity.Type == MessageEntityType.BotCommand)
                {
                    var command = msg.EntityValues.ElementAt(i);
                    if(string.Equals(command, CommandName))
                    {
                        Execute(client, update);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
