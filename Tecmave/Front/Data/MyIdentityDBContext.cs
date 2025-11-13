using Front.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tecmave.Front.Models;

namespace Front.Data
{
    public class MyIdentityDBContext
        : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public MyIdentityDBContext(DbContextOptions<MyIdentityDBContext> options)
            : base(options) { }

        public DbSet<Colaborador> Colaboradores { get; set; } = default!;
        public DbSet<Planilla> Planillas { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relación Planilla -> Colaborador (FK ColaboradorId)
            builder.Entity<Planilla>()
                   .HasOne(p => p.Colaborador)
                   .WithMany() // o .WithMany("Planillas") si agregas ICollection<Planilla> en Colaborador
                   .HasForeignKey(p => p.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
