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
        private readonly IConfiguration _config;

        public MessageController(IBot bot, ILoggerFactory loggerFactory, IConfiguration config)
        {
            _bot = bot;
            _logger = loggerFactory.CreateLogger<MessageController>();
            _config = config;
        }

        [HttpGet]
        [Route(@"api/message/start")]
        public string Start()
        {
            var url = string.Format(_config["Url"], @"api/message/update");
            _bot.Start(url);
            return "Bot Started";
        }

        [HttpPost]
        [Route(@"api/message/update")]
        public async Task<OkResult> Update([FromBody] Update update)
        {
            _logger.LogInformation("Handle update");
            await _bot.HandleUpdateAsync(update);
            return Ok();
        }
    }
}
