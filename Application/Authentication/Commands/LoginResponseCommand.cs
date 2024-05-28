using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class LoginResponseCommand
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Expiration {  get; set; }
    }
}
