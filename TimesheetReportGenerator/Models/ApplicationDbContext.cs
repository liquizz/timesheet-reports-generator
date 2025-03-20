using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TimesheetReportGenerator.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TicketTracking> TicketTrackings { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();

        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfiguration Configuration = builder.Build();

        optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), 
            new MySqlServerVersion(new Version(8, 0, 21)));

        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure all value types to be nullable
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {

                if (property.ClrType.IsValueType && Nullable.GetUnderlyingType(property.ClrType) == null)
                {
                    continue;
                }

                property.IsNullable = true;
            }
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}