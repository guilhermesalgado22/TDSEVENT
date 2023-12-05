using Microsoft.AspNetCore.Mvc;
using PlanejaiBack.Data;
using PlanejaiBack.Models;

namespace PlanejaiBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        [HttpGet]
        [Route("/Activities/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Activities!.OrderBy(a => a.ScheduleId).ToList());
        }

        [HttpGet("/Activities/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var activity = context.Activities!.FirstOrDefault(a => a.ActivityId == id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [HttpGet("/ActivitiesBySchedule/{id:int}")]
        public IActionResult GetBySchedule([FromRoute] int id, [FromServices] AppDbContext context)
        {
            return Ok(context.Activities!.Where(a => a.Schedule!.ScheduleId == id).OrderBy(a => a.StartDate).ThenBy(a => a.StartsAt).ToList());
        }

        [HttpPost("/Activities/")]
        public IActionResult Post([FromBody] ActivityModel activity, [FromServices] AppDbContext context)
        {
            var existingActivity = context.Activities!.SingleOrDefault(a => a.Name == activity.Name && a.ScheduleId == activity.ScheduleId);

            var sameTime = context.Activities!.SingleOrDefault(a => a.StartDate == activity.StartDate && a.StartsAt == activity.StartsAt
                                                                    && a.EndDate == activity.EndDate && a.EndsAt == activity.EndsAt
                                                                    && a.ScheduleId == activity.ScheduleId);

            if (sameTime != null)
            {
                return BadRequest("Já existe uma atividade marcada para este horário.");
            }

            if (existingActivity == null)
            {
                context.Activities!.Add(activity);
                context.SaveChanges();

                return Created($"/{activity.ActivityId}", activity);
            }

            return BadRequest("A atividade já foi adicionada ao cronograma.");
        }

        [HttpPut("/EditActivity/{activityId:int}")]
        public IActionResult Put([FromRoute] int activityId, [FromBody] ActivityModel activity, [FromServices] AppDbContext context)
        {
            if (activityId == activity.ActivityId)
            {
                context.Activities!.Update(activity);
                context.SaveChanges();

                return Ok(activity);
            }

            return NotFound();
        }

        [HttpDelete("/Activities/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var activity = context.Activities!.Find(id);

            if (activity != null)
            {
                context.Activities!.Remove(activity);
                context.SaveChanges();

                return Ok(activity);
            }

            return NotFound("A atividade já foi removida.");
        }
    }
}
