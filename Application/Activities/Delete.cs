using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public abstract class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                if (activity == null)
                    throw new ActivityNotFoundException();

                _context.Activities.Remove(activity);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return Unit.Value;

                throw new DbUpdateException("Problem saving changes");
            }
        }
    }
}