using Microsoft.EntityFrameworkCore;
using GREPO;

namespace DALMSQLXG
{
    public class Context : DbContext
    {
        public Context() : base()
        {
            Database.EnsureCreated();
        }
        public DbSet<WSRef>? WSRefs { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=zhenyakh; Initial Catalog=DAL; TrustServerCertificate=True; Integrated Security=True;");
        }
    }
}
