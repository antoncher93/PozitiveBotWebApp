using Pozitive.Entities;
using Pozitive.Entities.Repos;
using Pozitive.Services.Handlers.BotCommands;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers.BotCommands
{
    public class ReloadChatUpdateHandler : BotCommandHandler
    {
        private readonly IAdminService _adminService;
        protected override string Name { get; } = "/reload_chat";

        public ReloadChatUpdateHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }


        protected override void Execute(ITelegramBotClient client, Update update)
        {
            var message = update.Message;
            if(message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
            {
                _adminService.ReloadChat(message.Chat);
            }
        }
    }
}
