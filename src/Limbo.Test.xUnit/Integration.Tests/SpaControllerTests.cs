using Limbo.Test.xUnit.TestHelpers;
using Xunit;

namespace Limbo.Test.xUnit.Integration.Tests;

public class SpaControllerIntegrationTests {
    private readonly HttpClient _client;

    public SpaControllerIntegrationTests() {
        // Use TestHelper to dynamically load the WebApplicationFactory
        var factory = TestHelper.GetProjectProgram();
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost:10280/umbraco/api/spa/GetData?apphost=localhost&navLevels=2&&navContext=true&url=/da/limbotestarea/articlepage-form/&parts=content,site");
    }

    [Fact]
    public async Task Endpoint_Should_Return_Content() {
        System.Console.WriteLine($"Client Base Address: {_client.BaseAddress}");
        System.Console.WriteLine($"Client Default Request Headers: {_client.DefaultRequestHeaders}");
        System.Console.WriteLine("Testing example endpoint...");

        // Example test for an endpointa/GetDat
        var response = await _client.GetAsync("");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }
}