using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public abstract class CurrentUser
    {
        public class Query : IRequest<User>
        {
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;

            public Handler([NotNull] UserManager<AppUser> userManager, [NotNull] IUserAccessor userAccessor, [NotNull] IJwtGenerator jwtGenerator)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
                _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetCurrentUsername();
                var user = await _userManager.FindByNameAsync(username);
                if (user != default) return MapAppUserToUser(user);
                throw new RestException(HttpStatusCode.InternalServerError, new {Error = "Something gone wrong"});
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