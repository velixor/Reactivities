using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivitiesController([NotNull] IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<Activity> Single([FromRoute] Details.Query detailsQuery)
        {
            return await _mediator.Send(detailsQuery);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] Create.Command createActivityCommand)
        {
            return await _mediator.Send(createActivityCommand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Delete.Command deleteCommand)
        {
            return await _mediator.Send(deleteCommand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, [FromBody] Update.Command command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }
    }
}