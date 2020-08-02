using System.Net;

namespace Application.Errors
{
    public class UsernameAlreadyExistException : RestException
    {
        public UsernameAlreadyExistException() : base(HttpStatusCode.BadRequest, new {Username = "Username already exist"})
        {
        }
    }
}