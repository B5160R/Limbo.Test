using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

public abstract class IntegrationTestBase<TProgram> where TProgram : class {

    protected CustomWebApplicationFactory<TProgram> WebsiteFactory;
    protected AsyncServiceScope Scope { get; private set; }
    protected IServiceProvider ServiceProvider => Scope.ServiceProvider;

    protected virtual CustomWebApplicationFactory<TProgram> CreateApplicationFactory() {
        return new CustomWebApplicationFactory<TProgram>();
    }

    [OneTimeSetUp]
    public virtual void Setup() {
        WebsiteFactory = CreateApplicationFactory();
        Scope = WebsiteFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
    }

    [OneTimeTearDown]
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

    protected virtual async Task<string> GetContentAsStringAsync(string url) {
        var response = await Client.GetAsync(url);
        return await response.Content.ReadAsStringAsync()
            ?? throw new InvalidOperationException("Response content is null");
    }

    protected virtual TType GetService<TType>() where TType : notnull
        => ServiceProvider.GetRequiredService<TType>();

    protected virtual string GetBackofficeContentElements(string pageName) {
        var _entityService = GetService<IEntityService>();
        var _contentService = GetService<IContentService>();

        var entityContent = _entityService.GetAll<IContent>()
            .FirstOrDefault(x => x.Name == pageName) ?? throw new InvalidOperationException($"Content not found for page: {pageName}");
        var content = _contentService.GetById(entityContent.Id) ?? throw new InvalidOperationException($"Content not found page: {pageName}");
        var backofficeContentElements = content.GetValue<string>("contentElements") ?? throw new InvalidOperationException($"Content not found page: {pageName}");

        return backofficeContentElements;
    }

    protected virtual IOptions<GlobalSettings> GetGlobalSettings()
        => Scope.ServiceProvider.GetRequiredService<IOptions<GlobalSettings>>();

}