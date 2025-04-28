using System.Net;
using Herningsholm.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Skybrud.Essentials.Collections.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Media.EmbedProviders;
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
    [Description("Test to ensure that the GetData method in the SpaApiController returns expected data -- that is the registered data in Umbraco backoffice.")]
    public async Task GetData_ReturnsOKWithContentType() {
        // Arrange
        string pageName = "Alle block-elementer";
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/limbotestarea/test-underforside/alle-block-elementer/&parts=content,site";
        var jsonHandler = new JsonHandler();

        // Act
        // Get the content from the Umbraco backoffice
        var entityContent = _entityService.GetAll<IContent>()
            .FirstOrDefault(x => x.Name == pageName) ?? throw new InvalidOperationException($"Content not found for page: {pageName}");
        var content = _contentService.GetById(entityContent.Id) ?? throw new InvalidOperationException($"Content not found page: {pageName}");
        var backofficeContentElements = content.GetValue<string>("contentElements") ?? throw new InvalidOperationException($"Content not found page: {pageName}");

        // Get the content from the SPA API controller
        var spaGetDataResponse = await GetContentAsStringAsync(url);

        // Parse the JSON strings to JObjects
        var backofficeContentElementsParsedJson = jsonHandler.ConvertJsonToJObject(backofficeContentElements);
        var spaResponseParsedJson = jsonHandler.ConvertJsonToJObject(spaGetDataResponse);

        // Assert
        Assert.That(spaResponseParsedJson, Is.Not.Null, "Response is null.");
        Assert.That(spaResponseParsedJson, Is.Not.Empty, "Response is empty.");
        Assert.That(backofficeContentElementsParsedJson, Is.Not.Null, "Backoffice content is null.");
        Assert.That(backofficeContentElementsParsedJson, Is.Not.Empty, "Backoffice content is empty.");

        // This focuses on the elements on the document (page) only (ie. 'contentData' and 'contentElements')
        var backofficeTokens = backofficeContentElementsParsedJson.SelectTokens("$..contentData[?(@.udi)]");
        var spaResponseTokens = spaResponseParsedJson.SelectTokens("$..contentElements[?(@.key)]");

        JToken? spaElementRoot = null;
        string udiKey = string.Empty;

        Assert.Multiple(() => {
            foreach (var backofficeToken in backofficeTokens) {

                udiKey = backofficeToken.SelectToken("udi")?.ToString() ?? string.Empty;
                udiKey = udiKey.Replace("umb://element/", string.Empty);

                spaElementRoot = spaResponseTokens.FirstOrDefault(e => e.SelectToken("key")?.ToString().Replace("-", string.Empty) == udiKey);
                Assert.That(spaElementRoot, Is.Not.Null, $"Element with key {udiKey} not found in SPA response.");

                var backOfficeSubLevelTokens = jsonHandler.InspectForUdi(backofficeToken);
                var spaSubLevelElements = jsonHandler.InspectForKey(spaElementRoot);

                int backOfficeSubLevelCount = backOfficeSubLevelTokens.Count;
                foreach (var backOfficeSubLevelToken in backOfficeSubLevelTokens) {
                    var subLevelUdiKey = backOfficeSubLevelToken.SelectToken("udi")?.ToString() ?? string.Empty;
                    subLevelUdiKey = subLevelUdiKey.Replace("umb://element/", string.Empty);

                    int spaSubLevelCount = spaSubLevelElements.Count;
                    foreach (var spaSubLevelElement in spaSubLevelElements) {
                        var subLevelSpaKey = spaSubLevelElement.SelectToken("key")?.ToString() ?? string.Empty;
                        subLevelSpaKey = subLevelSpaKey.Replace("-", string.Empty);

                        if (subLevelUdiKey == subLevelSpaKey) {
                            Assert.That(subLevelSpaKey, Is.EquivalentTo(subLevelUdiKey), $"Sub-level element with key {subLevelUdiKey} not found in SPA response.");
                            // Remove the matched element to avoid duplicate checks
                            spaSubLevelElements.Remove(spaSubLevelElement);
                            // Exit the loop if a match is found
                            break;
                        }

                        spaSubLevelCount--;
                        if (spaSubLevelCount == 0) {
                            Assert.Fail($"Sub-level element with key {subLevelUdiKey} not found in SPA response.");
                        }
                    }
                }
            }
        });
    }

    [Test]
    [TestCase(TestName = "SpaApiControllerTests - GetData returns expected data for block elements")]
    [Description("Test to ensure that the GetData method in the SpaApiController returns expected data for block elements.")]
    public async Task GetData_ReturnsOKWithBlockElementContentType() {
        // Arrange
        string pageName = "Alle block-elementer";
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/da/limbotestarea/test-underforside/alle-block-elementer/&parts=content,site";
        var jsonHandler = new JsonHandler();

        var isNotSettingsData = new IsSettingsData(_invert: true);
        var isNotPage = new IsPage(_invert: true);
        ContentCheckBase<TokensContainer> contentCheck = ContentCheckBuilder
            .Create<SpaResponseBlockElementCheck, TokensContainer>(ServiceProvider)
            .WithRequirement(isNotSettingsData)
            .WithRequirement(isNotPage)
            .WithProjectInitials("LIMBO") // TODO: Fix this to not use project initials
            .Build();

        // Act
        // Get the content from the Umbraco backoffice
        var entityContent = _entityService.GetAll<IContent>()
            .FirstOrDefault(x => x.Name == pageName) ?? throw new InvalidOperationException($"Content not found for page: {pageName}");
        var content = _contentService.GetById(entityContent.Id) ?? throw new InvalidOperationException($"Content not found page: {pageName}");
        var backofficeContentElements = content.GetValue<string>("contentElements") ?? throw new InvalidOperationException($"Content not found page: {pageName}");

        // Get the content from the SPA API controller
        var spaGetDataResponse = await GetContentAsStringAsync(url);

        // Parse the JSON strings to JObjects
        var backofficeContentElementsParsedJson = jsonHandler.ConvertJsonToJObject(backofficeContentElements);
        var spaResponseParsedJson = jsonHandler.ConvertJsonToJObject(spaGetDataResponse);

        // Assert
        // Assert.That(spaResponseParsedJson, Is.Not.Null, "Response is null.");
        // Assert.That(spaResponseParsedJson, Is.Not.Empty, "Response is empty.");
        // Assert.That(backofficeContentElementsParsedJson, Is.Not.Null, "Backoffice content is null.");
        // Assert.That(backofficeContentElementsParsedJson, Is.Not.Empty, "Backoffice content is empty.");

        // This focuses on the elements on the document (page) only (ie. 'contentData' and 'contentElements')
        var backofficeTokens = backofficeContentElementsParsedJson.SelectTokens("$..contentData[?(@.udi)]");
        var spaResponseTokens = spaResponseParsedJson.SelectTokens("$..contentElements[?(@.key)]");

        await contentCheck.RunAssertionsAsync(backofficeTokens, spaResponseTokens);
    }
}