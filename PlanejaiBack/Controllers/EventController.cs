using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanejaiBack.Data;
using PlanejaiBack.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PlanejaiBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpGet]
        [Route("/Events/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Events!.OrderBy(e => e.StartDate).ToList());
        }

        [HttpGet("/Events/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var eventModel = context.Events!.SingleOrDefault(e => e.EventId == id);

            if (eventModel == null)
            {
                return NotFound();
            }

            return Ok(eventModel);
        }

        [HttpGet("/EventsByUser/{id:int}")]
        public IActionResult GetByUser([FromRoute] int id, [FromServices] AppDbContext context)
        {
            return Ok(context.Events!.Where(e => e.Organizer!.UserId == id).OrderBy(e => e.StartDate).ToList());
        }

        [HttpPost("/Events/")]
        public IActionResult Post([FromBody] EventModel eventModel, [FromServices] AppDbContext context)
        {
            var user = context.Users!.Include(u => u.Events).FirstOrDefault(u => u.UserId == eventModel.OrganizerId);
            var existingEvent = context.Events!.SingleOrDefault(e => e.Name == eventModel.Name);

            if (existingEvent == null)
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                };

                var jsonEvent = JsonSerializer.Serialize(eventModel, options);
                var deserializedEvent = JsonSerializer.Deserialize<EventModel>(jsonEvent, options);

                context.Events!.Add(deserializedEvent!);
                context.SaveChanges();

                return Created($"/{eventModel.EventId}", eventModel);
            }

            return BadRequest("O evento já foi criado!");
        }

        [HttpPut("/EditEvent/{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] EventModel eventModel, [FromServices] AppDbContext context)
        {
            if (id == eventModel.EventId)
            {
                context.Events!.Update(eventModel);
                context.SaveChanges();

                return Ok(eventModel);
            }

            return NotFound();
        }

        [HttpDelete("/Events/{eventId:int}")]
        public IActionResult Delete([FromRoute] int eventId, [FromServices] AppDbContext context)
        {
            var eventModel = context.Events!.Find(eventId);

            if (eventModel != null)
            {
                context.Events!.Remove(eventModel);
                context.SaveChanges();

                return Ok(eventModel);
            }

            return NotFound("O evento já foi removido.");
        }
    }
}
