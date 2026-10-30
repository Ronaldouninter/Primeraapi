
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIEventos.Models;

namespace APIEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Lugar { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; }

        [Range(0, int.MaxValue)]
        public int CupoDisponible { get; set; }

        [JsonIgnore]
        public List<Participante> Participantes { get; set; }
            = new();
    }
}
