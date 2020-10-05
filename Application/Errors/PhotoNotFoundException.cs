using System.Net;

namespace Application.Errors
{
    public class PhotoNotFoundException : RestException
    {
        public PhotoNotFoundException() : base(HttpStatusCode.NotFound, new {Photo = "Not found"})
        {
        }
    }
}