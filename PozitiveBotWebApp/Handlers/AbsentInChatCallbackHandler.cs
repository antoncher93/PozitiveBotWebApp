using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PozitiveBotWebApp.Handlers
{
    public class AbsentInChatCallbackHandler : UpdateHandler
    {
        public override async void Handle(ITelegramBotClient client, Update update)
        {
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                && string.Equals(update.CallbackQuery.Data, Bot.DONT_EXIST_IN_CHAT_REPLY))
            {
                var msg = update.CallbackQuery.Message;
                client.SendTextMessageAsync(msg.Chat.Id, "Я постараюсь исправиться. Давайте попробуем все сначала.")
                    .Wait();

                await Bot.AskUserWantIntoChatAsync(client, update.CallbackQuery.From.Id);
            }
            else
                base.Handle(client, update);
        }

    }
}
