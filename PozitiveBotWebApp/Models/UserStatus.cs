using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PozitiveBotWebApp.Models
{
    public enum UserStatus
    {
        Unknown = 0,
        Waiting = 1,
        Accepted = 2,
        Rejected = 3,
        WaitingPhotoOfDoc = 4,
        ExistInChat = 5,
    }
}
