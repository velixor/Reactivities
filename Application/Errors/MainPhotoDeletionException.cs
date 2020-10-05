using System.Net;

namespace Application.Errors
{
    public class MainPhotoDeletionException : RestException
    {
        public MainPhotoDeletionException() : base(HttpStatusCode.BadRequest, new {Photo = "You cannot delete main photo"})
        {
        }
    }
}