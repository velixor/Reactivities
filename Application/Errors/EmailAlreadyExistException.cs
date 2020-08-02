using System.Net;

namespace Application.Errors
{
    public class EmailAlreadyExistException : RestException
    {
        public EmailAlreadyExistException() : base(HttpStatusCode.BadRequest, new {Email = "Email already exist"})
        {
        }
    }
}