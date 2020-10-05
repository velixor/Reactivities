using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using JetBrains.Annotations;
using MediatR;
using Persistence;

namespace Application.Photos
{
    public abstract class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler([NotNull] DataContext context, [NotNull] IUserAccessor userAccessor, [NotNull] IPhotoAccessor photoAccessor)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
                _photoAccessor = photoAccessor ?? throw new ArgumentNullException(nameof(photoAccessor));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetCurrentUser();
                var photo = user.Photos.SingleOrDefault(x => x.Id == request.Id);

                if (photo == default) throw new PhotoNotFoundException();

                if (photo.IsMain) throw new MainPhotoDeletionException();

                _context.Photos.Remove(photo);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                var deletionResult = await _photoAccessor.DeletePhoto(photo.Id);

                if (isSavedSuccess && deletionResult == PhotoDeletionResult.Ok) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}