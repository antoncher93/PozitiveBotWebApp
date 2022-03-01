using Pozitive.Entities.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Pozitive.Services.Handlers.BotCommands
{
    class LoadDocumentCommandHandler : BotCommandHandler
    {
        private readonly IAdminService _adminService;
        private readonly IRepository<Entities.Document> _documents;

        protected override string Name { get; } = "/load_doc";

        public LoadDocumentCommandHandler(IAdminService adminService, IRepository<Pozitive.Entities.Document> documents)
        {
            _adminService = adminService;
            _documents = documents;
        }

        protected override void Execute(ITelegramBotClient client, Update update)
        {
            var msg = update.Message;

            if (_adminService.IsAdmin(msg.From.Id))
            {
                var id = int.Parse(msg.Text.Split(" ")[1]);
                var doc = _documents.GetAll().FirstOrDefault(d => d.PersonId == id);
                if (doc != null)
                {
                    string path = doc.File;
                    if (System.IO.File.Exists(path))
                    {
                        using (var stream = new FileStream(path, FileMode.Open))
                        {
                            var fileName = Path.GetFileName(doc.File);
                            var file = new InputOnlineFile(stream, fileName);
                            client.SendDocumentAsync(msg.Chat.Id, file).Wait();
                        }
                    }
                }
            }
        }
    }
}
