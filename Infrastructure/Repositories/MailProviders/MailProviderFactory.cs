using Application.Common.Interfaces;
using Application.Mail.Enums;
using Application.Mail.Interfaces;
using Application.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.MailProviders
{
    public class MailProviderFactory : IMailProviderFactory
    {
        public IMailServiceProvider GetProvider(Settings request)
        {
            IMailServiceProvider provider = null;
            switch (request.Provider)
            {
                case MailServiceProvider.Smtp:
                    provider = new Smtp();
                    break;
                case MailServiceProvider.Setrow:
                    provider = new Setrow();
                    break;
                default:
                    break;
            }

            return provider;
        }
    }
}
