using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PozitiveBotWebApp.Models;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly ITelegramBotClient _client;
        private readonly ApplicationContext _db;

        public MessageController(ITelegramBotClient client, ApplicationContext appContext, IConfiguration configuration, ILogger<Bot> logger)
        {
            _client = client;
            _db = appContext;
            Bot.Configure(appContext, configuration, logger);

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
