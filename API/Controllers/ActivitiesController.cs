using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActivityDto> Single([FromRoute] Details.Query detailsQuery)
        {
            return await Mediator.Send(detailsQuery);
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] Create.Command createActivityCommand)
        {
            return await Mediator.Send(createActivityCommand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Delete.Command deleteCommand)
        {
            return await Mediator.Send(deleteCommand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, [FromBody] Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult<Unit>> Attend([FromRoute] Attend.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}/attend")]
        public async Task<ActionResult<Unit>> Unattend([FromRoute] Unattend.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}