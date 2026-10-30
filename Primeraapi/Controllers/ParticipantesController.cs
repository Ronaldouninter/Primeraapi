
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIEventos.Data;
using APIEventos.Models;

namespace Primeraapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParticipantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Consultar todos los participantes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var participantes = await _context.Participantes.ToListAsync();
            return Ok(participantes);
        }

        // GET: Consultar participante por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var participante = await _context.Participantes.FindAsync(id);

            if (participante == null)
                return NotFound("Participante no encontrado.");

            return Ok(participante);
        }

        // POST: Registrar participante
        [HttpPost]
        public async Task<IActionResult> Post(Participante participante)
        {
            if (string.IsNullOrWhiteSpace(participante.NombreCompleto))
                return BadRequest("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(participante.CorreoElectronico))
                return BadRequest("El correo es obligatorio.");

            var evento = await _context.Eventos
                .FindAsync(participante.EventoId);

            if (evento == null)
                return BadRequest("El evento no existe.");

            if (evento.CupoDisponible <= 0)
                return Conflict("El evento no tiene cupo disponible.");

            evento.CupoDisponible--;

            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = participante.Id },
                participante);
        }

        // PUT: Actualizar participante
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Participante datos)
        {
            var participante = await _context.Participantes.FindAsync(id);

            if (participante == null)
                return NotFound("Participante no encontrado.");

            var nuevoEvento = await _context.Eventos
                .FindAsync(datos.EventoId);

            if (nuevoEvento == null)
                return BadRequest("El evento no existe.");

            if (participante.EventoId != datos.EventoId)
            {
                if (nuevoEvento.CupoDisponible <= 0)
                    return Conflict("El nuevo evento no tiene cupo.");

                var eventoAnterior = await _context.Eventos
                    .FindAsync(participante.EventoId);

                if (eventoAnterior != null)
                    eventoAnterior.CupoDisponible++;

                nuevoEvento.CupoDisponible--;
            }

            participante.NombreCompleto = datos.NombreCompleto;
            participante.CorreoElectronico = datos.CorreoElectronico;
            participante.EventoId = datos.EventoId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: Eliminar participante
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var participante = await _context.Participantes.FindAsync(id);

            if (participante == null)
                return NotFound("Participante no encontrado.");

            var evento = await _context.Eventos
                .FindAsync(participante.EventoId);

            if (evento != null)
                evento.CupoDisponible++;

            _context.Participantes.Remove(participante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
