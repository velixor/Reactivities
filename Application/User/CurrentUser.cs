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
            private readonly IAppUserAccessor _userAccessor;

            public Handler([NotNull] IAppUserAccessor userAccessor, [NotNull] IJwtGenerator jwtGenerator)
            {
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
                _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetCurrentUser();
                return MapAppUserToUser(user);
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