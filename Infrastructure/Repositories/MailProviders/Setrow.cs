using Application.Mail.Interfaces;
using Application.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MailProviders
{
    public class Setrow : IMailServiceProvider
    {
        public Task Send(Settings settings, string messageString, string titleString)
        {
            throw new NotImplementedException();
        }
    }
}
