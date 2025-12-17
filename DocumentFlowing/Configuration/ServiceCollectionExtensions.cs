using DocumentFlowing.Client;
using DocumentFlowing.Client.Authorization;
using DocumentFlowing.Client.Authorization.Services;
using DocumentFlowing.Helpers;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Services;
using DocumentFlowing.Views;
using DocumentFlowing.Views.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentFlowing.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Config
        services.AddSingleton(configuration);
        services.AddHttpClient();
        services.AddAppConfiguration();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(AuthorizationMappingProfile).Assembly);
        });

        // Services
        services.AddScoped<IGeneralClient, GeneralClient>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IAuthorizationClient, AuthorizationClient>();
        services.AddScoped<IDpapiService, DpapiService>();
        
        
        // Views / ViewModels
        services.AddSingleton<MainWindow>();
        services.AddTransient<LoginWindow>();
        services.AddTransient<AdminMainView>();

        return services;
    }
}