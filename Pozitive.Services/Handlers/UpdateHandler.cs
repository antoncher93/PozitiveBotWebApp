using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers
{
    public class UpdateHandler
    {
        private UpdateHandler _next;

        public UpdateHandler SetNext(UpdateHandler handler)
        {
            _next = handler;
            return handler;
        }

        public virtual void Handle(ITelegramBotClient client, Update update)
        {
            _next?.Handle(client, update);
        }

        public static UpdateHandler Create(IAdminService adminService, IRepository<Person> persons)
        {
            var tail = new PhotoHandler(adminService, persons);

            return tail;
        }
    }
}
