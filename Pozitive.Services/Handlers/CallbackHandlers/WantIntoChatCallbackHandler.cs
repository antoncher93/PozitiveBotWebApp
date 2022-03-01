using System.Linq;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Internal;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public class WantIntoChatCallbackHandler : CallbackHandler
    {
        private readonly IBot _bot;

        public override string Data => Bot.WANT_INTO_CHAT_CALLBACK_Q;

        public WantIntoChatCallbackHandler(IBot bot)
        {
            _bot = bot;
        }

        public override void Handle(ITelegramBotClient client, Update update)
        {
            if(string.Equals(update.CallbackQuery.Data, Bot.WANT_INTO_CHAT_CALLBACK_Q))
            {
                var from = update.CallbackQuery.From;

                _bot.BeginInvite(from.Id);
            }
            else base.Handle(client, update);
        }
    }
}
