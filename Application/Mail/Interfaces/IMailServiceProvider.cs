using Application.Mail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mail.Interfaces
{
    public interface IMailServiceProvider
    {
        Task Send(Settings settings, string defination, string subject);
    }
}
