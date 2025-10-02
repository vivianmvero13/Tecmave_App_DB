using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tecmave.Front.Models;

namespace Front.Data
{
    public class MyIdentityDBContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {

        public MyIdentityDBContext(DbContextOptions<MyIdentityDBContext> options)
            : base(options)
        {

        }


    }
    
}
