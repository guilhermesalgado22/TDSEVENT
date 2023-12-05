using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlanejaiBack.Models
{
    public class ScheduleModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Informe um nome.")]
        public string? Name { get; set; }

        public int EventId { get; set; }
        public EventModel? Event { get; set; }

        public List<ActivityModel> Activities { get; set; } = new List<ActivityModel>();
    }
}
