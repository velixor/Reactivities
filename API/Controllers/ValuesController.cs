using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        [HttpGet]
        [PublicAPI]
        public async Task<ActionResult<IEnumerable<Value>>> GetAsync()
        {
            var values = await _dataContext.Values.ToListAsync().ConfigureAwait(false);
            return Ok(values);
        }

        [HttpGet("{id}")]
        [PublicAPI]
        public async Task<ActionResult<Value>> GetActionAsync(int id)
        {
            var value = await _dataContext.Values.SingleAsync(x => x.Id == id).ConfigureAwait(false);
            return Ok(value);
        }
    }
}