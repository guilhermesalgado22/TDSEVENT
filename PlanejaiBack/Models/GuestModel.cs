using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlanejaiBack.Models
{
    public class GuestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestId { get; set; }

        [Required(ErrorMessage = "Informe seu nome.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Informe seu sobrenome.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Informe um número de telefone.")]
        public string? PhoneNumber { get; set; }

        public List<EventsGuests>? EventsGuests { get; set; } = new List<EventsGuests>();
    }
}
