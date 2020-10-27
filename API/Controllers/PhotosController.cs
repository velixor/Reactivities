using System;
using System.Threading.Tasks;
using Application.Photos;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Photo>> Add([FromForm] Add.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Delete.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("{id}/setMain")]
        public async Task<ActionResult<Unit>> Name([FromRoute] SetMain.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}