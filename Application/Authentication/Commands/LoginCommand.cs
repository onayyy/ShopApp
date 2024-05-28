using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class LoginCommand : IRequest<UserAggregate>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public class Handler : IRequestHandler<LoginCommand, UserAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;
            private readonly IPasswordHasherService _passwordHasherService;
 

            public Handler(IPizzaAppDbContext dbContext, IPasswordHasherService passwordHasherService)
            {
                _dbContext = dbContext;
                _passwordHasherService = passwordHasherService;
            }

            public async Task<UserAggregate> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    throw new Exception("Kullanıcı veya şifre hatalı"); 
                }

                var isPasswordValid = _passwordHasherService.VerifyPassword(request.Password, user.Password);

                if (!isPasswordValid)
                {
                    throw new Exception("Kullanıcı veya şifre hatalı");
                }

                return user;
            }
        }
    }
}
