using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Positive.SqlDbContext.Repos;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers;
using Pozitive.Services.Handlers.CallbackHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Internal
{
    public class Bot : IBot
    {
        public const string WANT_INTO_CHAT_CALLBACK_Q = "DONT_WANT_INTO_CHAT";
        public const string DONT_WANT_INTO_CHAT_CALLBACK_Q = "WANT_INTO_CHAT";
        public const string ADD_USER_INTO_CHAT = "ADD_USER_INTO_CHAT";
        public const string EXIST_IN_CHAT_REPLY = "EXIST_IN_CHAT_REPLY";
        public const string DONT_EXIST_IN_CHAT_REPLY = "DONT_EXIST_IN_CHAT_REPLY";
        public const long TargetChatId = 0;
        public const string Name = "PozitiveBot";

        private const string Text = "Доброго времени суток.\nЖелаете вступить в закрытый чат 7-го корпуса ЖК Позитив?";
        private const string Token = "5043762282:AAG7khmTE87EZO483tU7yDBikYiwbB4j-rY";

        internal const string APPROVE_USER = "ACCEPT_USER";
        internal const string REJECT_USER = "REJECT_USER";

        private readonly ITelegramBotClient _client;
        private UpdateHandler _rootUpdateHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Person> _persons;

        public Bot(IServiceProvider serviceProvider, UserRepos persons)
        {
            _serviceProvider = serviceProvider;
            _client = serviceProvider.GetService<ITelegramBotClient>();
            _persons = persons;

            _ConfigureHandlers();
        }

        public void Start(string hook)
        {
            _client.SetWebhookAsync(hook, allowedUpdates: new[] { UpdateType.CallbackQuery, UpdateType.Message });
        }

        private void _ConfigureHandlers()
        {
            _rootUpdateHandler = new StartUpdateHandler(_persons);
            //_rootUpdateHandler
            //    .SetNext(new WantIntoChatCallbackHandler(appContext))
            //    .SetNext(new ReloadAdminCommandHandler(appContext, configuration))
            //    .SetNext(new ReloadChatUpdateHandler(configuration))
            //    .SetNext(new PhotoHandler(appContext, configuration))
            //    .SetNext(new ApproveCalbackHandler(configuration, appContext))
            //    .SetNext(new RejectUserHandler(configuration, appContext));
        }

        public void HandleUpdate(Update update)
        {
            if (update is null)
                return;

            try
            {
                _rootUpdateHandler.Handle(_client, update);
            }
            catch (Exception exc)
            {
                //_logger.LogError(exc.Message + Environment.NewLine + exc.StackTrace);
            }

        }

        public async Task HandleUpdateAsync(Update update)
        {
            await Task.Factory.StartNew(() => HandleUpdate(update));
        }


        public static async Task AskUserWantIntoChatAsync(ITelegramBotClient client, long chatId)
        {
            var button = InlineKeyboardButton.WithCallbackData("Да", Bot.WANT_INTO_CHAT_CALLBACK_Q);
            var keyboard = new InlineKeyboardMarkup(new[] { button });
            await client.SendTextMessageAsync(chatId, Text, ParseMode.Markdown, replyMarkup: keyboard);
        }
    }
}
