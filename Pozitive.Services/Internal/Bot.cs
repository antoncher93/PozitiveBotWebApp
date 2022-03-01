using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Positive.SqlDbContext.Repos;
using Pozitive.Entities;
using Pozitive.Entities.Enums;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers;
using Pozitive.Services.Handlers.AdminCommands;
using Pozitive.Services.Handlers.BotCommands;
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

        private const string InviteText1 = "Хорошо, для того, чтобы стать участником закрытого чата 7 корпуса ЖК Позитив, необходимо подтвердить свою принадлежность. "
                                   + "Для этого пришлите мне фото документа, подтверждающего собственость или участие в долевом "
                                   + "строительстве (фото первой страницы ДДУ или т.п.)";
        private const string InviteText2 = "Отправьте фото первой страницы ДДУ или фото акта приемки УК";

        private readonly ITelegramBotClient _client;
        private UpdateHandler _rootUpdateHandler;
        private BotCommandHandler _botCommandHandler;
        private CallbackHandler _callbackHandler;
        private readonly IRepository<Person> _persons;
        private readonly IRepository<Entities.Document> _documents;
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;

        public Bot(ITelegramBotClient client, IAdminService adminService, 
            UserRepos persons, 
            DocumentRepos documents,
            ILoggerFactory loggerFactory)
        {
            _client = client;
            _persons = persons;
            _documents = documents;
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
            _callbackHandler = CallbackHandler.Create(this, _adminService);
            _botCommandHandler = BotCommandHandler.Create(_adminService, _persons, _documents);
            _rootUpdateHandler = UpdateHandler.Create(_adminService, _persons);
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
                else if(update.Type == UpdateType.CallbackQuery)
                {
                    _callbackHandler.Handle(_client, update);
                }
                else
                {
                    _rootUpdateHandler?.Handle(_client, update);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message + Environment.NewLine + exc.StackTrace);
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

        public void BeginInvite(long userId)
        {
            var person = _persons.GetAll()
                .FirstOrDefault(u => long.Equals(u.TelegramId, userId));

            person.Status = UserStatus.WaitingPhotoOfDoc;
            _persons.Update(person);

            _client.SendTextMessageAsync(person.ChatId, InviteText1);
            _client.SendTextMessageAsync(person.ChatId, InviteText2);
        }
    }
}
