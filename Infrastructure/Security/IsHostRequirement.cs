using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dataContext;
        private readonly HttpContext _httpContext;

        public IsHostRequirementHandler([NotNull] IHttpContextAccessor httpContext, [NotNull] DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _httpContext = httpContext.HttpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            if (await IsHostRequirementSucceed())
                context.Succeed(requirement);
            else
                context.Fail();
        }

        private async Task<bool> IsHostRequirementSucceed()
        {
            var username = _httpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username == default) return false;

            var activityIdString = _httpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString();
            if (string.IsNullOrWhiteSpace(activityIdString)) return false;
            if (!Guid.TryParse(activityIdString, out var activityId)) return false;

            return await _dataContext.UserActivities.AnyAsync(x =>
                x.AppUser.UserName == username &&
                x.ActivityId == activityId &&
                x.IsHost);
        }
    }
}