
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIEventos.Data;
using APIEventos.Models;

namespace Primeraapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Consultar todos los eventos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var eventos = await _context.Eventos.ToListAsync();
            return Ok(eventos);
        }

        // GET: Consultar evento por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound("Evento no encontrado.");

            return Ok(evento);
        }

        // POST: Registrar evento
        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            if (string.IsNullOrWhiteSpace(evento.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (evento.CupoDisponible < 0)
                return BadRequest("El cupo no puede ser negativo.");

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = evento.Id },
                evento);
        }

        // PUT: Actualizar evento
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento datos)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound("Evento no encontrado.");

            if (string.IsNullOrWhiteSpace(datos.Nombre))
                return BadRequest("El nombre es obligatorio.");

            if (datos.CupoDisponible < 0)
                return BadRequest("El cupo no puede ser negativo.");

            evento.Nombre = datos.Nombre;
            evento.Lugar = datos.Lugar;
            evento.Fecha = datos.Fecha;
            evento.CupoDisponible = datos.CupoDisponible;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: Eliminar evento
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound("Evento no encontrado.");

            bool tieneParticipantes = await _context.Participantes
                .AnyAsync(p => p.EventoId == id);

            if (tieneParticipantes)
                return Conflict(
                    "No se puede eliminar un evento con participantes.");

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
