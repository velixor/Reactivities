using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public abstract class Unattend
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
                var user = await _userAccessor.GetCurrentUser();
                var attendance = await _context.UserActivities.SingleOrDefaultAsync(x =>
                        x.AppUserId == user.Id &&
                        x.ActivityId == request.Id, cancellationToken)
                    ?? throw new RestException(HttpStatusCode.NotFound, new {Attendance = "Attendance not found"});

                if (attendance.IsHost)
                    throw new RestException(HttpStatusCode.BadRequest, new {Attendance = "User cannot unattend hosted activity"});

                _context.UserActivities.Remove(attendance);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}