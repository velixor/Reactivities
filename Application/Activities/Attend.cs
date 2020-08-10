using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public abstract class Attend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEqual(Guid.Empty);
            }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler([NotNull] DataContext context, [NotNull] IUserAccessor userAccessor)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id)
                    ?? throw new RestException(HttpStatusCode.NotFound, new {Activity = "Could not find activity"});

                var user = await _userAccessor.GetCurrentUser();

                await ThrowIfUserAlreadyAttendActivity(user, activity, cancellationToken);

                var userActivity = BuildAttendance(user, activity);
                await _context.UserActivities.AddAsync(userActivity, cancellationToken);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }

            private async Task ThrowIfUserAlreadyAttendActivity(AppUser user, Activity activity, CancellationToken cancellationToken)
            {
                var isUserActivityExist = await _context.UserActivities.AnyAsync(x =>
                    x.AppUserId == user.Id &&
                    x.ActivityId == activity.Id, cancellationToken);

                if (isUserActivityExist)
                    throw new RestException(HttpStatusCode.BadRequest, new {UserActivity = "User already attend this activity"});
            }

            private UserActivity BuildAttendance(AppUser user, Activity activity)
            {
                return new UserActivity
                {
                    AppUser = user,
                    Activity = activity,
                    DateJoined = DateTime.Now,
                    IsHost = false
                };
            }
        }
    }
}