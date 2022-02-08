using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PozitiveBotWebApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly ITelegramBotClient _client;

        public MessageController(IServiceProvider serviceProvider)
        {
            _client = serviceProvider.GetService<ITelegramBotClient>();
            Bot.Configure(serviceProvider);

        }

        [HttpPost]
        [Route(@"api/message/update")]
        public async Task<OkResult> Update([FromBody] Update update)
        {
            await Bot.HandleUpdateAsync(_client, update);
            return Ok();
        }
    }
}
