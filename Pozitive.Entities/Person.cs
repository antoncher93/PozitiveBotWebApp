using Pozitive.Entities.Enums;

namespace Pozitive.Entities
{
    public class Person
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
