using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);

        public bool VerifyPassword(string password, string hashedPassword);
    }
}
