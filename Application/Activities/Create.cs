using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public abstract class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; [UsedImplicitly] set; }
            public string Title { get; [UsedImplicitly] set; }
            public string Description { get; [UsedImplicitly] set; }
            public string Category { get; [UsedImplicitly] set; }
            public DateTime Date { get; [UsedImplicitly] set; }
            public string City { get; [UsedImplicitly] set; }
            public string Venue { get; [UsedImplicitly] set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Venue).NotEmpty();
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
                var activity = MapRequestToActivity(request);
                var userActivity = await GetUserActivity(activity);

                await _context.Activities.AddAsync(activity, cancellationToken);
                await _context.UserActivities.AddAsync(userActivity, cancellationToken);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }

            private static Activity MapRequestToActivity(Command request)
            {
                return new Activity
                {
                    Id = request.Id,
                    Category = request.Category,
                    City = request.City,
                    Date = request.Date,
                    Description = request.Description,
                    Title = request.Title,
                    Venue = request.Venue
                };
            }

            private async Task<UserActivity> GetUserActivity(Activity activity)
            {
                return new UserActivity
                {
                    Activity = activity,
                    AppUser = await _userAccessor.GetCurrentUser(),
                    DateJoined = DateTime.Now,
                    IsHost = true
                };
            }
        }
    }
}