using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {

    public HttpClient CreateCustomClient(Uri baseAddress) {
        HttpClient client = CreateClient();
        // Set the base address for the client
        client.BaseAddress = baseAddress;

        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.Test.json");

        builder.ConfigureAppConfiguration(conf => {
            conf.AddJsonFile(configPath);
        });

        builder.ConfigureServices((context, services) => {
            var configuration = context.Configuration;
            services.AddSingleton(configuration);
        });

        builder.ConfigureServices(services => {
            // Remove all hosted services to prevent background jobs from starting
            var hostedServices = services.Where(descriptor =>
                descriptor.ServiceType == typeof(IHostedService)).ToList();

            foreach (var hostedService in hostedServices) {
                services.Remove(hostedService);
            }

            // Replace specific hosted services with mock implementations?
            // services.AddSingleton<IHostedService, MockHostedService>();
        });
    }
}