using System;
using Microsoft.Extensions.DependencyInjection;
using Pozitive.Entities;
using Pozitive.Entities.Repos;
using PozitiveBotWebApp.Handlers.CallbackHandlers;

namespace Pozitive.Services.Handlers.CallbackHandlers
{
    public static class CallbackHandlerExtension
    {
        public static UpdateHandler AddCallBackHandlers(this UpdateHandler rootHandler, IServiceProvider provider)
        {
            var persons = provider.GetService<IRepository<Person>>();
            var adminService = provider.GetService<IAdminService>();

            return rootHandler.SetNext(new ApproveCallbackHandler(adminService, persons))
                .SetNext(new DontWantIntoChatCallbackHandler(persons))
                .SetNext(new WantIntoChatCallbackHandler(persons))
                .SetNext(new ExistInChatCallbackHandler(persons));
        }
    }
}
