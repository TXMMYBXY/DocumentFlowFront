using DocumentFlowing.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;

namespace DocumentFlowing.Helpers;

public static class ConfigurationHelper
{
    private static IConfiguration _configuration;
    
    public static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                _InitializeConfiguration();
            }
            return _configuration;
        }
    }

    public static IConfiguration GetConfiguration()
    {
        if (_configuration == null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            _configuration = builder.Build();
        }
        
        return _configuration;
    }
    
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services)
    {
        var configuration = GetConfiguration();
        
        services.Configure<DocumentFlowApi>(
            configuration.GetSection(nameof(DocumentFlowApi)));
        
        services.AddSingleton(sp => 
            sp.GetRequiredService<IOptions<DocumentFlowApi>>().Value);
        
        return services;
    }
    
    private static void _InitializeConfiguration()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();
    }
}