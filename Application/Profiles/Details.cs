using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public abstract class Details
    {
        public class Query : IRequest<Profile>
        {
            public string UserName { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler([NotNull] DataContext context, [NotNull] IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

                return _mapper.Map<AppUser, Profile>(user);
            }
        }
    }
}