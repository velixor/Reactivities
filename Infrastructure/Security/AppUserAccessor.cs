using System;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public class AppUserAccessor : IAppUserAccessor
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;

        public AppUserAccessor([NotNull] IUserAccessor userAccessor, UserManager<AppUser> userManager)
        {
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
            _userManager = userManager;
        }

        public async Task<AppUser> GetCurrentUser()
        {
            var username = _userAccessor.GetCurrentUsername();
            return await _userManager.FindByNameAsync(username)
                ?? throw new RestException(HttpStatusCode.InternalServerError, new {AppUser = "User not found"});
        }
    }
}