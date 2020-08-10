using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAccessor(UserManager<AppUser> userManager, [NotNull] IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task<AppUser> GetCurrentUser()
        {
            var username = GetCurrentUsername();
            return await _userManager.FindByNameAsync(username)
                ?? throw new RestException(HttpStatusCode.InternalServerError, new {AppUser = "User not found"});
        }

        private string GetCurrentUsername()
        {
            return _contextAccessor.HttpContext
                .User
                ?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}