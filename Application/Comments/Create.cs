using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Comments
{
    public abstract class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
            public string UserName { get; set; }
            public DateTime CreatedAt { get; } = DateTime.Now;
        }

        [UsedImplicitly]
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;

            public Handler([NotNull] DataContext context, [NotNull] IMapper mapper, [NotNull] UserManager<AppUser> userManager)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                var comment = await BuildComment(request, user);
                user.Comments.Add(comment);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return _mapper.Map<CommentDto>(comment);

                throw new Exception("Problem saving changes");
            }

            private async Task<Comment> BuildComment(Command request, AppUser user)
            {
                var activity = await _context.Activities.FindAsync(request.ActivityId);
                if (activity == null) throw new ActivityNotFoundException();

                return new Comment
                {
                    Activity = activity,
                    Author = user,
                    Body = request.Body,
                    CreatedAt = request.CreatedAt
                };
            }
        }
    }
}