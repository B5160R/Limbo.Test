using System.Net;
using Herningsholm.Web;
using NUnit.Framework;
using Skybrud.Essentials.Collections.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[Category("Integration")]
[Description("Integration tests for validating SPA API controller return data.")]
[TestOf(typeof(SpaApiControllerTests))]
public class SpaApiControllerTests : IntegrationTestBase<Program> {

    private IContentTypeService _contentTypeService;
    private IEntityService _entityService;
    private IContentService _contentService;
    private IDataTypeService _dataTypeService;

    [SetUp]
    public override void Setup() {
        base.Setup();
        _contentTypeService = GetService<IContentTypeService>();
        _entityService = GetService<IEntityService>();
        _contentService = GetService<IContentService>();
        _dataTypeService = GetService<IDataTypeService>();

    }

    [Test]
    [TestCase(TestName = "SpaApiControllerTests - GetData returns 200 OK")]
    [Description("Test to ensure that the GetData method in the SpaApiController returns a 200 OK response.")]
    public async Task GetData_ReturnsOK() {
        // Arrange
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/&parts=content,site";

        // Act
        var response = await GetAsync<HttpResponseMessage>(url);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Expected 200 OK response.");
    }

    [Test]
    [TestCase(TestName = "SpaApiControllerTests - GetData returns expected data")]
    [Description("Test to ensure that the GetData method in the SpaApiController returns expected data.")]
    public async Task GetData_ReturnsOKWithContentType() {
        // Arrange
        string pageName = "Alle block-elementer";
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/limbotestarea/alle-block-elementer/&parts=content,site";

        // Act
        var entityContent = _entityService.GetAll<IContent>()
            .FirstOrDefault(x => x.Name == pageName) ?? throw new InvalidOperationException($"Content not found for page: {pageName}");
        var content = _contentService.GetById(entityContent.Id) ?? throw new InvalidOperationException($"Content not found page: {pageName}");
        var result = content.GetValue<string>("contentElements");

        var contentType = _contentTypeService.Get(content.ContentTypeId) ?? throw new InvalidOperationException($"Content type not found for page: {pageName}");
        var contentTypeProperties = contentType.PropertyTypes.ToList();
        var contentElementsProperties = new List<IPropertyType>();
        foreach (var property in contentTypeProperties) {
            if (property.Alias == "contentElements") {
                contentElementsProperties.Add(property);
            }
        }

        var elements = _contentTypeService.Get(contentElementsProperties.FirstOrDefault().Id);

        // var model = await GetAsync<Welcome>(url);
        var response = await GetContentAsStringAsync(url);

        // Assert
        Assert.That(response, Is.Not.Null, "Response is null.");
        Assert.That(response, Is.Not.Empty, "Response is empty.");

    }
}