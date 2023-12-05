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
    public class ScheduleController : ControllerBase
    {
        [HttpGet]
        [Route("/Schedules/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Schedules!.OrderBy(s => s.ScheduleId).ToList());
        }

        [HttpGet("/Schedules/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var schedule = context.Schedules!.Include(e => e.Event).FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        [HttpGet("/SchedulesByUser/{id:int}")]
        public IActionResult GetByUser([FromRoute] int id, [FromServices] AppDbContext context)
        {
            return Ok(context.Schedules!.Include(e => e.Event).Where(s => s.Event!.OrganizerId == id).OrderBy(s => s.Event!.StartDate).ThenBy(s => s.Event!.StartsAt).ToList());
        }

        [HttpPost("/Schedules/")]
        public IActionResult Post([FromBody] ScheduleModel schedule, [FromServices] AppDbContext context)
        {
            var existingSchedule = context.Schedules!.SingleOrDefault(s => (s.Name == schedule.Name && s.EventId == schedule.EventId) || s.EventId == schedule.EventId);

            if (existingSchedule == null)
            {
                context.Schedules!.Add(schedule!);
                context.SaveChanges();

                return Created($"/{schedule!.ScheduleId}", schedule);
            }

            return BadRequest("Um cronograma já foi adicionado para este evento.");
        }

        [HttpPut("/EditSchedule/{scheduleId:int}")]
        public IActionResult Put([FromRoute] int scheduleId, [FromBody] ScheduleModel schedule, [FromServices] AppDbContext context)
        {
            if (scheduleId == schedule.ScheduleId)
            {
                context.Schedules!.Update(schedule);
                context.SaveChanges();

                return Ok(schedule);
            }

            return NotFound();
        }

        [HttpDelete("/Schedules/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var schedule = context.Schedules!.Find(id);

            if (schedule != null)
            {
                context.Schedules!.Remove(schedule);
                context.SaveChanges();

                return Ok(schedule);
            }

            return NotFound("O cronograma já foi removido.");
        }
    }
}
