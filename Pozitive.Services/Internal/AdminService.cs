using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pozitive.Entities;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pozitive.Services.Internal
{
    class AdminService : IAdminService
    {
        private readonly ITelegramBotClient _client;
        private long _adminChatId;
        private long _mainChatId;

        public AdminService(ITelegramBotClient client)
        {
            _client = client;
        }

        public bool IsAdmin(Person person)
        {
            var member = _client.GetChatMemberAsync(_mainChatId, person.TelegramId).Result;
            return member.Status == ChatMemberStatus.Creator
                   && member.Status == ChatMemberStatus.Administrator;
        }

        public void ForwardDocumentToAdmin(Person person, string photoFileId)
        {
            throw new NotImplementedException();
        }

        public void ReloadChat(long chatId)
        {
            throw new NotImplementedException();
        }

        public void DeclinePerson(User admin, long userId)
        {
            throw new NotImplementedException();
        }

        public void InvitePerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void ForwardDocumentToAdmin(ITelegramBotClient client, Person person, string photoFileId)
        {
            var buttonYes = InlineKeyboardButton.WithCallbackData("Принять", Bot.APPROVE_USER);
            var buttonNo = InlineKeyboardButton.WithCallbackData("Отклонить", Bot.REJECT_USER);
            var keyboard = new InlineKeyboardMarkup(new[] { buttonYes, buttonNo });

            var telegramId = person.TelegramId;
            var mention = "[" + person.Id + "](tg://user?id=" + telegramId + ")";
            var text = $"Пользователь {mention} хочет в чат";
            client.SendPhotoAsync(_adminChatId, photoFileId , text, ParseMode.Markdown, replyMarkup: keyboard);

        }
    }
}
