using System;
using System.Linq;
using System.Security.Claims;
using Application.Interfaces;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAccessor([NotNull] IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public string GetCurrentUsername()
        {
            return _contextAccessor.HttpContext
                .User
                ?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}