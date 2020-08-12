using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Photos
{
    public abstract class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler([NotNull] DataContext context, [NotNull] IUserAccessor userAccessor, [NotNull] IPhotoAccessor photoAccessor)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
                _photoAccessor = photoAccessor ?? throw new ArgumentNullException(nameof(photoAccessor));
            }

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccessor.AddPhoto(request.File);
                var user = await _userAccessor.GetCurrentUser();
                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;

                user.Photos.Add(photo);

                var isSavedSuccess = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (isSavedSuccess) return photo;

                throw new Exception("Problem saving changes");
            }
        }
    }
}