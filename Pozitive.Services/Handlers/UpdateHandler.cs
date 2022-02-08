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
    }
}
