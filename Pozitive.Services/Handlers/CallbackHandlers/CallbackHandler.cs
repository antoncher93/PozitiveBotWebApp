using Pozitive.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public abstract class CallbackHandler
    {
        private CallbackHandler _next;
        public abstract string Data { get; }
        public virtual void Handle(ITelegramBotClient client, Update update)
        {
            _next?.Handle(client, update);
        }
        public CallbackHandler SetNext(CallbackHandler next)
        {
            _next = next;
            return _next;
        }
        public static CallbackHandler Create(IBot bot, IAdminService adminService)
        {
            var head = new WantIntoChatCallbackHandler(bot);
            head.SetNext(new ApproveCallbackHandler(adminService))
                .SetNext(new RejectUserHandler(adminService));
            return head;
        }
    }
}
