using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public AdminService(ITelegramBotClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public bool IsAdmin(User user, Chat chat)
        {
            var member = _client.GetChatMemberAsync(chat, user.Id).Result;
            return member.Status == ChatMemberStatus.Creator
                   && member.Status == ChatMemberStatus.Administrator;
        }

        public void ForwardDocumentToAdmin(Person person, string photoFileId)
        {
            var buttonYes = InlineKeyboardButton.WithCallbackData("Принять", Bot.APPROVE_USER);
            var buttonNo = InlineKeyboardButton.WithCallbackData("Отклонить", Bot.REJECT_USER);
            var keyboard = new InlineKeyboardMarkup(new[] { buttonYes, buttonNo });

            var telegramId = person.TelegramId;
            var mention = "[" + person.Id + "](tg://user?id=" + telegramId + ")";
            var text = $"Пользователь {mention} хочет в чат";
            var adminChatId = _configuration.GetValue<long>("AdminChatId");
            _client.SendPhotoAsync(adminChatId, photoFileId, text, ParseMode.Markdown, replyMarkup: keyboard);
        }

        public async void ReloadChat(long chatId)
        {
            _configuration["MainChatId"] = chatId.ToString();
            await _client.SendTextMessageAsync(chatId, "Настройка завершена.");
        }

        public void DeclinePerson(User admin, long userId)
        {
            
        }

        public void InvitePerson(Person person)
        {
            
        }

        public bool IsChatMember(long telegramId)
        {
            var member = _client.GetChatMemberAsync(_configuration.GetValue<long>("MainChatId"), telegramId).Result;
            return member.Status != ChatMemberStatus.Left && member.Status != ChatMemberStatus.Kicked;
        }
    }
}
