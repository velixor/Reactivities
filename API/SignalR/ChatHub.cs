using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Comments;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub([NotNull] IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task SendComment(Create.Command command)
        {
            command.UserName = UserName();

            var comment = await _mediator.Send(command);

            await Clients.All.SendAsync("ReceiveComment", comment);
        }

        // TODO Move logic to IUserAccessor
        private string UserName()
        {
            return Context.User
                ?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}