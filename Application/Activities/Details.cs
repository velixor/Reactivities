using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Errors;
using AutoMapper;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public abstract class Details
    {
        public class Query : IRequest<ActivityDto>
        {
            public Guid Id { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler([NotNull] DataContext context, [NotNull] IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id)
                    ?? throw new ActivityNotFoundException();

                return _mapper.Map<Activity, ActivityDto>(activity);
            }
        }
    }
}