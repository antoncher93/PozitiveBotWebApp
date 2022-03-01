using System;
using System.Reflection.Metadata;
using Pozitive.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Pozitive.Services
{
    public interface IAdminService
    {
        bool IsAdmin(long telegramId);
        void ForwardDocumentToAdmin(Person person, string photoFileId);
        void ReloadChat(Chat chat);
        void DeclinePerson(User admin, Person person);
        void InvitePerson(int personId, PhotoSize photo);
        bool IsChatMember(long telegramId);
    }
}
