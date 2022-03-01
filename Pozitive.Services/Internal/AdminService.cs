using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Positive.SqlDbContext.Repos;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using Pozitive.Entities.Enums;

namespace Pozitive.Services.Internal
{
    class AdminService : IAdminService
    {
        private readonly ITelegramBotClient _client;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Entities.Document> _documents;
        private readonly IRepository<Entities.Person> _persons;

        public AdminService(ITelegramBotClient client, IConfiguration configuration,
            DocumentRepos documents,
            UserRepos persons)
        {
            _client = client;
            _configuration = configuration;
            _documents = documents;
            _persons = persons;
        }

        public bool IsAdmin(long telegramId)
        {
            return long.Equals(telegramId, _configuration.GetValue<long>("AdminId"));
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

        public async void ReloadChat(Chat chat)
        {
            _configuration["MainChatId"] = chat.Id.ToString();
            await _client.SendTextMessageAsync(chat.Id, "Настройка завершена.");
        }

        public void DeclinePerson(User admin, long userId)
        {
            
        }

        public void InvitePerson(int personId, PhotoSize photo)
        {
            var person = _persons.GetAll().FirstOrDefault(u => Equals(u.Id, personId));
            if (person is null)
                return;

            person.Status = UserStatus.Normal;
            _persons.Update(person);

            var chat = _client.GetChatAsync(_configuration.GetValue<long>("MainChatId")).Result;
            var link = _client.CreateChatInviteLinkAsync(chat).Result.InviteLink;
            person.InviteLink = link;

            _persons.Update(person);

            var text = $"Подтверждение принято. Можете вступить в закрытый чат 7 корпуса ЖК Позитив по ссылке:\n" + link;
            _client.SendTextMessageAsync(person.ChatId, text);

            var extension = ".jpg";
            var filePath = $"Documents\\{personId}{extension}";
            var directory = System.IO.Path.GetDirectoryName(filePath);

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            var mode = System.IO.File.Exists(filePath) 
                ? System.IO.FileMode.Truncate 
                : System.IO.FileMode.Create;

            using (var stream = new System.IO.FileStream(filePath, mode))
            {
                var file = _client.GetFileAsync(photo.FileId).Result;
                _client.DownloadFileAsync(file.FilePath, stream)
                    .Wait();
            }

            var document = _documents.GetAll().FirstOrDefault(d => Equals(d.PersonId, person.Id));
            if (document is null)
            {
                document = new Entities.Document()
                {
                    PersonId = person.Id,
                    File = filePath
                };

                _documents.Add(document);
            }
            else
            {
                document.File = filePath;
                _documents.Update(document);
            }
        }

        public bool IsChatMember(long telegramId)
        {
            var member = _client.GetChatMemberAsync(_configuration.GetValue<long>("MainChatId"), telegramId).Result;
            return member.Status != ChatMemberStatus.Left && member.Status != ChatMemberStatus.Kicked;
        }
    }
}
