using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Front.Models;
using Microsoft.AspNetCore.Identity;
using Tecmave.Front.Models;

namespace Front.Data
{
    public class MyIdentityDBContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public MyIdentityDBContext(DbContextOptions<MyIdentityDBContext> options)
            : base(options)
        {
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
    }
}
