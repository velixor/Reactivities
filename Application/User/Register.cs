using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Application.Validators;
using Domain;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public abstract class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [UsedImplicitly]
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly UserManager<AppUser> _userManager;

            public Handler([NotNull] DataContext context, [NotNull] UserManager<AppUser> userManager, [NotNull] IJwtGenerator jwtGenerator)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                await ThrowIfEmailOrUsernameAlreadyExist(request);

                var user = MapRequestToAppUser(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded) return MapAppUserToUser(user);

                throw new Exception("Problem creating user");
            }

            private async Task ThrowIfEmailOrUsernameAlreadyExist(Command request)
            {
                var (email, username) = (request.Email, request.Username);

                if (await _context.Users.AnyAsync(x => x.Email == email)) throw new EmailAlreadyExistException();
                if (await _context.Users.AnyAsync(x => x.UserName == username)) throw new UsernameAlreadyExistException();
            }

            private static AppUser MapRequestToAppUser(Command request)
            {
                return new AppUser
                {
                    DisplayName = request.DisplayName,
                    UserName = request.Username,
                    Email = request.Email
                };
            }

            private User MapAppUserToUser(AppUser user)
            {
                return new User
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
        }
    }
}