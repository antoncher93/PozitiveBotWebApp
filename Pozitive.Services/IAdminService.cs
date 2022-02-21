using System;
using System.Reflection.Metadata;
using Pozitive.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services
{
    public interface IAdminService
    {
        bool IsAdmin(User user, Chat chat);
        void ForwardDocumentToAdmin(Person person, string photoFileId);
        void ReloadChat(long chatId);
        void DeclinePerson(User admin, long userId);
        void InvitePerson(Person person);
        bool IsChatMember(long telegramId);
    }
}
