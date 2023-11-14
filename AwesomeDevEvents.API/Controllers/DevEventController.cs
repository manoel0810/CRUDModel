using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventController : ControllerBase
    {
        private readonly DevEventsDbContext _dbContext;

        public DevEventController(DevEventsDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var events = _dbContext.DevEvents.Where(e => !e.IsDeleted).ToList();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(Guid id)
        {
            var devEvent = _dbContext.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
                return NotFound();

            return Ok(devEvent);
        }

        [HttpPost]
        public ActionResult Post(DevEvent devEvent)
        {
            _dbContext.DevEvents.Add(devEvent);
            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _dbContext.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
                return NotFound();

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var devEvent = _dbContext.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
                return NotFound();

            devEvent.Delete();
            return NoContent();
        }
    }
}
