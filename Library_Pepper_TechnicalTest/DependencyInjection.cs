using LibraryPepper.API.Common;
using LibraryPepper.Domain.Repositories;
using LibraryPepper.Infrastructure.Context;
using LibraryPepper.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;
using System.Reflection;

namespace LibraryPepper.API
{
    public static class DependencyInjection
    {
        public static void AddInMemoryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryContext>(options =>
            {
                options.UseInMemoryDatabase("LibraryDB");
            });
        }

        public static void AddAppServices(this IServiceCollection services)
        {
            //services.AddSingleton<IApiEnvironment>(new ApiEnvironment(config, environment));
            AddMapster(services);
            AddMediatR(services);
            AddRepositories(services);
        }

        private static void AddMediatR(this IServiceCollection services)
        {
            Assembly application = AppDomain.CurrentDomain.Load("LibraryPepper.Application");
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(application);
            });
        }

        private static void AddMapster(this IServiceCollection services)
        {
            //mapster
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
        }
    }
}
