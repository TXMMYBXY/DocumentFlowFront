using DocumentFlowing.Client;
using DocumentFlowing.Views;
using DocumentFlowing.Views.Admin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace DocumentFlowing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Регистрируем HttpClient
            services.AddHttpClient();

            // Регистрируем ваш GeneralClient
            services.AddScoped<GeneralClient>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();

                // Настраиваем базовый адрес API
                var baseAddress = ConfigurationManager.ConnectionStrings["WebApiConnection"]?.ConnectionString;
                if (!string.IsNullOrEmpty(baseAddress))
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                }

                return new GeneralClient(httpClient);
            });

            // Регистрируем ваши окна
            services.AddTransient<LoginView>();
            services.AddTransient<AdminMainView>();
            // Добавьте другие окна по необходимости
        }
    }

}
