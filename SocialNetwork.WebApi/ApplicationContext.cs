using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace SocialNetwork.WebApi;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public ApplicationContext() { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = social_net_db; Username = postgres; Password = Ilyas255448");
    }
}
