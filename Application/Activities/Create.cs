using System;
using System.Threading;
using System.Threading.Tasks;
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

            public Handler([NotNull] DataContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = new Activity
                {
                    Id = request.Id,
                    Category = request.Category,
                    City = request.City,
                    Date = request.Date,
                    Description = request.Description,
                    Title = request.Title,
                    Venue = request.Venue
                };
                await _context.Activities.AddAsync(activity, cancellationToken);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}