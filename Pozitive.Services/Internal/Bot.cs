using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers;
using Pozitive.Services.Handlers.CallbackHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PozitiveBotWebApp
{
    public class Bot
    {
        public const string WANT_INTO_CHAT_CALLBACK_Q = "DONT_WANT_INTO_CHAT";
        public const string DONT_WANT_INTO_CHAT_CALLBACK_Q = "WANT_INTO_CHAT";
        public const string ADD_USER_INTO_CHAT = "ADD_USER_INTO_CHAT";

        public const string EXIST_IN_CHAT_REPLY = "EXIST_IN_CHAT_REPLY";
        public const string DONT_EXIST_IN_CHAT_REPLY = "DONT_EXIST_IN_CHAT_REPLY";

        public const long TargetChatId = 0;

        private const string Token = "5043762282:AAG7khmTE87EZO483tU7yDBikYiwbB4j-rY";
        private const string Url = "https://f44b-109-252-76-17.ngrok.io/{0}";
        public const string Name = "PozitiveBot";
        internal const string REJECT_USER = "REJECT_USER";
        private static TelegramBotClient _client;
        private static UpdateHandler _rootUpdateHandler;
        internal const string APPROVE_USER = "ACCEPT_USER";

        private const string Text = "Доброго времени суток.\nЖелаете вступить в закрытый чат 7-го корпуса ЖК Позитив?";

        internal const long ROOT_ADMIN_ID = 816204353L;

        public static void Configure(IServiceProvider serviceProvider)
        {
            var persons = serviceProvider.GetService<IRepository<Person>>();
            _rootUpdateHandler = new StartUpdateHandler(persons);
            //_rootUpdateHandler
            //    .SetNext(new WantIntoChatCallbackHandler(appContext))
            //    .SetNext(new ReloadAdminCommandHandler(appContext, configuration))
            //    .SetNext(new ReloadChatUpdateHandler(configuration))
            //    .SetNext(new PhotoHandler(appContext, configuration))
            //    .SetNext(new ApproveCalbackHandler(configuration, appContext))
            //    .SetNext(new RejectUserHandler(configuration, appContext));
        }

        public static void HandleUpdate(ITelegramBotClient client, Update update)
        {
            if (update is null)
                return;

            try
            {
                _rootUpdateHandler.Handle(client, update);
            }
            catch (Exception exc)
            {
                //_logger.LogError(exc.Message + Environment.NewLine + exc.StackTrace);
            }

        }

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update)
        {
            await Task.Factory.StartNew(() => HandleUpdate(client, update));
        }


        public static async Task AskUserWantIntoChatAsync(ITelegramBotClient client, long chatId)
        {
            var button = InlineKeyboardButton.WithCallbackData("Да", Bot.WANT_INTO_CHAT_CALLBACK_Q);
            var keyboard = new InlineKeyboardMarkup(new[] { button });
            await client.SendTextMessageAsync(chatId, Text, ParseMode.Markdown, replyMarkup: keyboard);
        }
    }
}
