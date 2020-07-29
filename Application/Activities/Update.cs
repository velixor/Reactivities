using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public abstract class Update
    {
        public class Command : IRequest
        {
            public Guid Id { get; [UsedImplicitly] set; }
            public string Title { get; [UsedImplicitly] set; }
            public string Description { get; [UsedImplicitly] set; }
            public string Category { get; [UsedImplicitly] set; }
            public DateTime? Date { get; [UsedImplicitly] set; }
            public string City { get; [UsedImplicitly] set; }
            public string Venue { get; [UsedImplicitly] set; }
        }

        public class CommandValidator : AbstractValidator<Create.Command>
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

            public Handler([NotNull] DataContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                if (activity == null) throw new Exception("Activity not found");

                activity.Category = request.Category ?? activity.Category;
                activity.City = request.City ?? activity.City;
                activity.Date = request.Date ?? activity.Date;
                activity.Description = request.Description ?? activity.Description;
                activity.Title = request.Title ?? activity.Title;
                activity.Venue = request.Venue ?? activity.Venue;

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}