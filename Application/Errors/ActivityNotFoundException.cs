using System.Net;

namespace Application.Errors
{
    public class ActivityNotFoundException : RestException
    {
        public ActivityNotFoundException() : base(HttpStatusCode.NotFound, new {activity = "Not found"})
        {
        }
    }
}