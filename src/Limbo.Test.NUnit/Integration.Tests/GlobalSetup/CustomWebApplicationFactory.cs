using Herningsholm.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;


// TODO: Find a way to set Program reference dynamically (not like now by using...)
public class CustomWebApplicationFactory : WebApplicationFactory<Program> {

    public HttpClient CreateCustomClient() {
        HttpClient client = CreateClient();
        client.BaseAddress = new Uri("https://localhost:10280/umbraco/");

        return client;
    }

    // private const string _inMemoryConnectionString = "Data Source=IntegrationTests;Mode=Memory;Cache=Shared";
    // private readonly SqliteConnection _imConnection;
    //
    // public CustomWebApplicationFactory() {
    //     _imConnection = new SqliteConnection(_inMemoryConnectionString);
    //     _imConnection.Open();
    // }
    //
    // protected override void ConfigureWebHost(IWebHostBuilder builder) {
    //     // TODO: Find out if env should be something else...
    //     // Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    //
    //     var projectDir = Directory.GetCurrentDirectory();
    //     var configPath = Path.Combine(projectDir, "appsettings.Tests.json");
    //
    //     builder.ConfigureAppConfiguration(conf => {
    //         conf.AddJsonFile(configPath);
    //         conf.AddInMemoryCollection(new KeyValuePair<string, string?>[]
    //         {
    //             new("ConnectionStrings:umbracoDbDSN", _inMemoryConnectionString),
    //             new("ConnectionStrings:umbracoDbDSN_ProviderName", "Microsoft.Data.Sqlite")
    //         });
    //     });
    // }
    //
    // protected override void Dispose(bool disposing) {
    //     base.Dispose(disposing);
    //
    //     // When this application factory is disposed, close the connection to the in-memory database
    //     _imConnection.Close();
    //     _imConnection.Dispose();
    // }

}