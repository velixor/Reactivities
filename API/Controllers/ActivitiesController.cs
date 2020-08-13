using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Activities;
using Application.Dtos;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SolutionConstants;

namespace API.Controllers
{
    public class ActivitiesController : BaseController
    {
        private readonly IDistributedCache _cache;

        public ActivitiesController([NotNull] IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActivityDto> Single([FromRoute] Details.Query query)
        {
            var cacheKey = query.Id.ToString();
            ActivityDto activity;
            string serializedActivity;
            
            var encodedActivity = await _cache.GetAsync(cacheKey);

            if (encodedActivity != null)
            {
                serializedActivity = Encoding.UTF8.GetString(encodedActivity);
                activity = JsonSerializer.Deserialize<ActivityDto>(serializedActivity);
            }
            else
            {
                activity = await Mediator.Send(query);
                serializedActivity = JsonSerializer.Serialize(activity);
                encodedActivity = Encoding.UTF8.GetBytes(serializedActivity);
                var option = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));
                await _cache.SetAsync(cacheKey, encodedActivity, option);
            }

            return activity;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] Create.Command createActivityCommand)
        {
            return await Mediator.Send(createActivityCommand);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Constants.IsActivityHostPolicy)]
        public async Task<ActionResult<Unit>> Delete([FromRoute] Delete.Command deleteCommand)
        {
            return await Mediator.Send(deleteCommand);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Constants.IsActivityHostPolicy)]
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