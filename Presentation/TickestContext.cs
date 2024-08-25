using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

namespace Tickest.Persistence
{
    public class TickestContext : DbContext
    {
        public TickestContext(DbContextOptions<TickestContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Regra> Regras { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRegra> UsuarioRegras { get; set; }
    }
}
