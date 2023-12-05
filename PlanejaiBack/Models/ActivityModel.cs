using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanejaiBack.Models
{
    public class ActivityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

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

        public int ScheduleId { get; set; }

        public ScheduleModel? Schedule { get; set; }

        public bool DatesAreValid()
        {
            if (EndDate.HasValue && EndsAt.HasValue && StartDate.HasValue && StartsAt.HasValue)
            {
                DateTime activityEndDateTime = EndDate.Value.Date + EndsAt.Value.TimeOfDay;
                DateTime activityStartDateTime = StartDate.Value.Date + StartsAt.Value.TimeOfDay;

                DateTime eventEndDateTime = Schedule!.Event!.EndDate!.Value.Date + Schedule!.Event!.EndsAt!.Value.TimeOfDay;
                DateTime eventStartDateTime = Schedule!.Event!.StartDate!.Value.Date + Schedule!.Event!.StartsAt!.Value.TimeOfDay;

                if (activityStartDateTime < eventStartDateTime || activityEndDateTime > eventEndDateTime)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
