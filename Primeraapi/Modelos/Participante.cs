
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIEventos.Models
{
    public class Participante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int EventoId { get; set; }

        [JsonIgnore]
        public Evento? Evento { get; set; }
    }
}
