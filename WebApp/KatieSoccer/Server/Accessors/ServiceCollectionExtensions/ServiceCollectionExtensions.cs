using KatieSoccer.Server.Accessors.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KatieSoccer.Server.Accessors.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAccessorDependencies(this IServiceCollection services)
        {
            services.AddTransient<IGameAccessor, GameAccessor>();

            services.AddTransient<IKatieSoccerDbContext, KatieSoccerDbContext>();
            services.AddDbContext<KatieSoccerDbContext>(options =>
                options.UseSqlite("Filename=KatieSoccer.db"));
        }
    }
}
