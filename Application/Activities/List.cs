using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public abstract class List
    {
        public class Query : IRequest<List<Activity>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;

            public Handler([NotNull] DataContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities.AsNoTracking().ToListAsync(cancellationToken);

                return activities;
            }
        }
    }
}