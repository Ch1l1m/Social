using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp3
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = social_net_db; Username = postgres; Password = Ilyas255448");
        }
    }
}
