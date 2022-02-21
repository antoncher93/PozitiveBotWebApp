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
using Pozitive.Services;

namespace PozitiveBotWebApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IBot _bot;
        private readonly ILogger _logger;

        public MessageController(IBot bot, ILoggerFactory loggerFactory)
        {
            _bot = bot;
            _logger = loggerFactory.CreateLogger<MessageController>();
        }

        [HttpPost]
        [Route(@"api/message/update")]
        public async Task<OkResult> Update([FromBody] Update update)
        {
            await _bot.HandleUpdateAsync(update);
            return Ok();
        }
    }
}
