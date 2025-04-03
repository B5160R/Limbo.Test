using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;
using Umbraco.Cms.Core.Configuration.Models;

public abstract class IntegrationTestBase {

    protected CustomWebApplicationFactory WebsiteFactory { get; private set; } = new();
    protected AsyncServiceScope Scope { get; private set; }
    protected IServiceProvider ServiceProvider => Scope.ServiceProvider;

    protected virtual CustomWebApplicationFactory CreateApplicationFactory() {
        return new CustomWebApplicationFactory();
    }

    [SetUp]
    public virtual void Setup() {
        WebsiteFactory = CreateApplicationFactory();
        Scope = WebsiteFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
    }

    [TearDown]
    public virtual void TearDown() {
        Scope.Dispose();
        WebsiteFactory.Dispose();
    }

    protected virtual HttpClient Client
        => WebsiteFactory.CreateCustomClient();

    protected virtual async Task<T> GetAsync<T>(string url) {
        var response = await Client.GetAsync(url);
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()
            ?? throw new InvalidOperationException("Response content is null"))!;
    }

    protected virtual TType GetService<TType>() where TType : notnull
        => ServiceProvider.GetRequiredService<TType>();

    protected virtual IOptions<GlobalSettings> GetGlobalSettings()
        => Scope.ServiceProvider.GetRequiredService<IOptions<GlobalSettings>>();

}