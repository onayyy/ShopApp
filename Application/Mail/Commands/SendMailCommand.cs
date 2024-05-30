using Application.Common.Interfaces;
using Application.Mail.Enums;
using Application.Mail.Interfaces;
using Application.Mail.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mail.Commands
{
    public class SendMailCommand : IRequest<Unit>
    {
        public SendMailCommand(string subject, string definition, Settings settings)
        {
            Subject = subject;
            Definition = definition;
            Settings = settings;
        }

        public string Subject { get; set; }
        public string Definition { get; set; }
        public Settings Settings { get; set; }

        public class Handler : IRequestHandler<SendMailCommand, Unit>
        {
            private readonly IMailProviderFactory _mailProviderFactory;

            public Handler(IMailProviderFactory mailProviderFactory)
            {
                _mailProviderFactory = mailProviderFactory;
            }

            public async Task<Unit> Handle(SendMailCommand request, CancellationToken cancellationToken)
            {
                IMailServiceProvider provider = _mailProviderFactory.GetProvider(request.Settings);
                if (provider == null)
                {
                    throw new Exception("PROVIDER_NOT_FOUND");
                }

                await provider.Send(request.Settings, request.Definition, request.Subject);

                return Unit.Value;
            }
        }
    }
}
