using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReactBoard1.Models
{
    public static class  EntryExtensions
    {
        public static void AddDependencyInjectionContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<EntryDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            services.AddTransient<IEntryRepository, EntryRepository>();
        }
    }
}    