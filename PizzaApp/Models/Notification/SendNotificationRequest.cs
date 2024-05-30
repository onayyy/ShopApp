using Application.Mail.Commands;
using Application.Mail.Models;

namespace ShopAPI.Models.Notification
{
    public class SendNotificationRequest
    {
        public string Subject { get; set; }
        public string Definition { get; set; }
        public Settings Settings { get; set; }

        public SendMailCommand ToCommand()
        {
            return new SendMailCommand(Subject, Definition, Settings);
        }
    }
}
