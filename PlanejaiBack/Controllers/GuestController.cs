using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanejaiBack.Data;
using PlanejaiBack.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlanejaiBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : Controller
    {
        [HttpGet]
        [Route("/Guests/")]
        public IActionResult Get([FromServices] AppDbContext context)
        {
            return Ok(context.Guests!.OrderBy(g => g.Name).ToList());
        }

        [HttpGet("/Guests/{id:int}")]
        public IActionResult GetByID([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var guest = context.Guests!.FirstOrDefault(g => g.GuestId == id);

            if (guest == null)
            {
                return NotFound();
            }

            return Ok(guest);
        }

        [HttpGet("/GuestsByEvent/{id:int}")]
        public IActionResult GetByEvent([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var guests = context.EventsGuests!.Where(eg => eg.EventId == id).ToList();

            if (guests == null)
            {
                return NotFound();
            }

            return Ok(guests);
        }

        [HttpPost("/AddGuest/{id:int}")]
        public IActionResult Post([FromRoute] int id, [FromBody] GuestModel guest, [FromServices] AppDbContext context)
        {
            var existingEvent = context.Events!.Find(id);

            var existingGuest = context.Guests!.FirstOrDefault(g => g.Email == guest.Email && (g.Name == guest.Name && g.LastName == guest.LastName));
            var otherGuest = context.Guests!.FirstOrDefault(g => g.Email == guest.Email && (g.Name != guest.Name || g.LastName != guest.LastName));

            var isIntheList = context.EventsGuests!.Include(eg => eg.Guest).SingleOrDefault(eg => eg.Guest!.Email == guest.Email && (eg.Guest!.Name == guest.Name && eg.Guest!.LastName == guest.LastName) && eg.EventId == id);

            if (isIntheList != null)
            {
                return BadRequest("O convidado informado já está na lista.");
            }

            if (otherGuest != null)
            {
                return BadRequest("Outro usuário já utiliza o e-mail informado.");
            }

            if (existingGuest == null)
            {
                existingEvent!.EventsGuests!.Add(new EventsGuests { Event = existingEvent, Guest = guest });

                context.SaveChanges();

                return Created($"/{guest.GuestId}", guest);
            }
            else
            {
                existingEvent!.EventsGuests!.Add(new EventsGuests { Event = existingEvent, Guest = existingGuest });

                context.SaveChanges();

                return Ok(existingGuest);
            }
        }

        [HttpPut("/Guests/{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] UserModel guest, [FromServices] AppDbContext context)
        {
            var existingGuest = context.Guests!.FirstOrDefault(g => g.GuestId == id);

            if (existingGuest != null)
            {
                context.Guests!.Update(existingGuest);
                context.SaveChanges();

                return Ok(existingGuest);
            }

            return NotFound();
        }

        [HttpDelete("/Guests/{eventId:int}/{guestId:int}")]
        public IActionResult Delete([FromRoute] int eventId, int guestId, [FromServices] AppDbContext context)
        {
            var eventsGuests = context.EventsGuests!.Find(eventId, guestId);

            if (eventsGuests != null)
            {
                context.EventsGuests!.Remove(eventsGuests);
                context.SaveChanges();

                return Ok(eventsGuests);
            }

            return NotFound("O convidado já foi removido.");
        }
    }
}
