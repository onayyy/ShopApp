using Application.Common.Interfaces;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.User.Commands
{
    public class AddUserCommand : IRequest<UserAggregate>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Gender { get; set; }


        public AddUserCommand(string name, string surname, string email, string password, int gender)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Gender = gender;
        }

        public class Handler : IRequestHandler<AddUserCommand, UserAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;
            private readonly IPasswordHasherService _passwordHasherService;

            public Handler(IPizzaAppDbContext dbContext, IPasswordHasherService passwordHasherService)
            {
                _dbContext = dbContext;
                _passwordHasherService = passwordHasherService;
            }

            public async Task<UserAggregate> Handle(AddUserCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Surname))
                    throw new Exception("USER_NAME_OR_SURNAME_CANNOT_BE_EMPTY");


                if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Password))
                    throw new Exception("USER_EMAIL_OR_PASSWORD_CANNOT_BE_EMPTY");

                var hashedPassword = _passwordHasherService.HashPassword(request.Password);

                var user = UserAggregate.Create(request.Name, request.Surname, request.Email, hashedPassword, request.Gender);
                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return user;
            }
        }
    }
}
