
using APIEventos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEventos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Participante> Participantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participante>()
                .HasOne(p => p.Evento)
                .WithMany(e => e.Participantes)
                .HasForeignKey(p => p.EventoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
