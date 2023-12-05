using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanejaiFront.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Informe seu nome.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Informe seu sobrenome.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "Informe um número de telefone.")]
        public string? PhoneNumber { get; set; }

        public List<EventModel>? Events { get; set; } = new List<EventModel>();
    }
}
