using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlanejaiBack.Models
{
    public class EventModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Informe um nome.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Informe uma data de início.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Informe um horário de início.")]
        public DateTime? StartsAt { get; set; }

        [Required(ErrorMessage = "Informe uma data de encerramento.")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Informe um horário de encerramento.")]
        public DateTime? EndsAt { get; set; }

        [Required(ErrorMessage = "Informe um local.")]
        public string? Local { get; set; }

        public int OrganizerId { get; set; }
        public UserModel? Organizer { get; set; }

        public int ScheduleId { get; set; }
        [JsonIgnore]
        public ScheduleModel? Schedule { get; set; }

        public List<EventsGuests>? EventsGuests { get; set; } = new List<EventsGuests>();

        public List<ActivityModel>? Activities { get; set; } = new List<ActivityModel>();

        public bool DatesAreValid ()
        {
            if (EndDate.HasValue && EndsAt.HasValue && StartDate.HasValue && StartsAt.HasValue)
            {
                DateTime endDateTime = EndDate.Value.Date + EndsAt.Value.TimeOfDay;
                DateTime startDateTime = StartDate.Value.Date + StartsAt.Value.TimeOfDay;

                if (endDateTime < startDateTime)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
