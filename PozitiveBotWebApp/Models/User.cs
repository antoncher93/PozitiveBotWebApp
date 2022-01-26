using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PozitiveBotWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public UserStatus Status { get; set; }
        public long? ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
