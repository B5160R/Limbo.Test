using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {

    public HttpClient CreateCustomClient() {
        HttpClient client = CreateClient();
        client.BaseAddress = new Uri("https://localhost:10280/umbraco/");

        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        var projectDir = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(projectDir, "appsettings.Tests.json");

        builder.ConfigureAppConfiguration(conf => {
            conf.AddJsonFile(configPath);
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
