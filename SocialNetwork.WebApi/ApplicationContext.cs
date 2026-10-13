using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SocialNetwork.WebApi;

public class ApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public ApplicationContext() { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration) : base(options) 
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
