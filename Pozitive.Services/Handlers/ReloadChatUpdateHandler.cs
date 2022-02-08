using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pozitive.Services.Handlers
{
    public class ReloadChatUpdateHandler : UpdateHandler
    {
        private readonly IAdminService _adminService;
        public ReloadChatUpdateHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public override void Handle(ITelegramBotClient client, Update update)
        {
            var message = update.Message;

            if (message?.Entities != null
                && message.Entities.Any()
                && message.Entities[0].Type == MessageEntityType.BotCommand
                && message.EntityValues.ElementAt(0) == "/reload_chat")
            {

                _adminService.ReloadChat(message.Chat.Id);

                //var member = client.GetChatMemberAsync(message.Chat.Id, message.From.Id).Result;
                //if ((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
                //    && (member.Status == ChatMemberStatus.Administrator || member.Status == ChatMemberStatus.Creator))
                //{
                //    var inviteLink = client.CreateChatInviteLinkAsync(message.Chat.Id).Result;

                //    _configuration["ChatInviteLink"] = inviteLink.InviteLink;
                //    var text = $"Настройка завершена";
                //    client.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Markdown);
                //}
                //else
                //{
                //    client.SendTextMessageAsync(message.Chat.Id, "Команда доступна только для администраторов группы");
                //}
            }
            else base.Handle(client, update); // next handler
        }
    }
}
