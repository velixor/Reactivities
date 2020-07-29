using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public abstract class Details
    {
        public class Query : IRequest<Activity>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _context;

            public Handler([NotNull] DataContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.FindAsync(request.Id)
                    ?? throw new ActivityNotFoundException();
            }
        }
    }
}