using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Positive.SqlDbContext.Repos;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers;
using Pozitive.Services.Handlers.AdminCommands;
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
        private UpdateHandler _botCommandHandler;
        private readonly IRepository<Person> _persons;
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;

        public Bot(ITelegramBotClient client, IAdminService adminService, UserRepos persons, ILoggerFactory loggerFactory)
        {
            _client = client;
            _persons = persons;
            _adminService = adminService;
            _logger = loggerFactory.CreateLogger<Bot>();

            _ConfigureHandlers();
        }

        public void Start(string hook)
        {
            try
            {
                _client.SetWebhookAsync(hook, allowedUpdates: new[] { UpdateType.CallbackQuery, UpdateType.Message });
                _logger.LogInformation($"Bot started at webhook {hook}");
            }
            catch(Exception exc)
            {
                _logger.LogError($"Error of SetWebhookAsync. {exc.Message}");
            }
            
        }

        private void _ConfigureHandlers()
        {
            _botCommandHandler = new StartUpdateHandler(_persons);
            _botCommandHandler.SetNext(new ReloadChatUpdateHandler(_adminService, _persons))
                .SetNext(new SendToAllAdminCommand(_adminService, _persons));

            _rootUpdateHandler = new RootUpdateHandler();
            //_rootUpdateHandler
            //    .SetNext()
            ////    .SetNext(new WantIntoChatCallbackHandler(appContext))
            ////    .SetNext(new ReloadAdminCommandHandler(appContext, configuration))
            //    .SetNext(new MessageToReloadChatHandler(_persons))
                
            ////    .SetNext(new PhotoHandler(appContext, configuration))
            ////    .SetNext(new ApproveCalbackHandler(configuration, appContext))
            ////    .SetNext(new RejectUserHandler(configuration, appContext));
        }

        public void HandleUpdate(Update update)
        {
            if (update is null)
                return;

            try
            {
                var msg = update.Message;
                if(msg != null && msg.Entities != null && msg.Entities.Any(e => e.Type == MessageEntityType.BotCommand))
                {
                    _botCommandHandler.Handle(_client, update);
                }
                else
                {
                    _rootUpdateHandler.Handle(_client, update);
                }

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
