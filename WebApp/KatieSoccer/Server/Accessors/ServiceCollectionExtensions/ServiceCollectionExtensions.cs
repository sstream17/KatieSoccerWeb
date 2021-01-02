using System;
using AutoMapper;
using KatieSoccer.Server.Accessors.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KatieSoccer.Server.Accessors.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAccessorDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IGameAccessor, GameAccessor>();

            services.AddTransient<IKatieSoccerDbContext, KatieSoccerDbContext>();
            services.AddDbContext<KatieSoccerDbContext>(options =>
                options.UseCosmos(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    databaseName: "GamesDB"));
        }
    }
}
