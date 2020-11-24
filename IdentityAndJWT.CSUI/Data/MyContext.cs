using IdentityAndJWT.CSUI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAndJWT.CSUI.Data
{
    public class MyContext : IdentityDbContext<User, Role, int>
    {
        public MyContext()
        {

        }
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"DataSource=/Users/erdinckara/Projects/IdentityAndJWT/IdentityAndJWT.db");


    }
}