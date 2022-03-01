using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Pozitive.Services
{
    public interface IBot
    {
        void Start(string baseUrl);
        void HandleUpdate(Update update);
        Task HandleUpdateAsync(Update update);
        void BeginInvite(long userId);
    }
}
