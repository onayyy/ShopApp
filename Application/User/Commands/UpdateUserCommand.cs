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
    public class UpdateUserCommand : IRequest<UserAggregate>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public int Gender { get; set; }

        public UpdateUserCommand(int id, string name, string surname, string password, int gender)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Password = password;
            Gender = gender;
        }

        public class Handler : IRequestHandler<UpdateUserCommand, UserAggregate>
        {
            private readonly IPizzaAppDbContext _dbContext;

            public Handler(IPizzaAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<UserAggregate> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users.FindAsync(request.Id, cancellationToken);

                if (user == null)
                    throw new Exception($"User with ID {request.Id} not found.");
     
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Surname) || string.IsNullOrWhiteSpace(request.Password))
                    throw new Exception($"User with ID {request.Id} not found.");

                user.Update(request.Name, request.Surname, request.Password, request.Gender);
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return user;
            }
        }


    }
}
