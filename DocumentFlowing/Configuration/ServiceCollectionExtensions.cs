using DocumentFlowing.Client;
using DocumentFlowing.Client.Authorization;
using DocumentFlowing.Helpers;
using DocumentFlowing.Interfaces.Client;
using DocumentFlowing.Interfaces.Client.Services;
using DocumentFlowing.Interfaces.Services;
using DocumentFlowing.Models;
using DocumentFlowing.Services;
using DocumentFlowing.ViewModels.Admin;
using DocumentFlowing.ViewModels.Authorization;
using DocumentFlowing.ViewModels.Boss;
using DocumentFlowing.ViewModels.Controls;
using DocumentFlowing.Views.Admin;
using DocumentFlowing.Views.Authorization;
using DocumentFlowing.Views.Boss;
using DocumentFlowing.Views.Controls;
using DocumentFlowing.Views.Purchaser;
using DocumentFlowing.Views.User;
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
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<ISessionProviderService, SessionProviderService>();
        
        // Models
        services.AddScoped<LoginModel>();
        
        // Views
        services.AddSingleton<MainWindow>();
        services.AddTransient<LoginView>();
        services.AddTransient<SidebarView>();
        services.AddTransient<AdminMainView>();
        services.AddTransient<BossMainView>();
        services.AddTransient<PurchaserMainView>();
        services.AddTransient<UserMainView>();
        
        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<SidebarViewModel>();
        services.AddTransient<AdminMainViewModel>();
        services.AddTransient<BossMainViewModel>();
        
        return services;
    }
}