namespace Core.Models
{
    public class MailSettingsModel
    {
        public required string Name { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Host { get; set; }
        public required int Port { get; set; }
    }
}
