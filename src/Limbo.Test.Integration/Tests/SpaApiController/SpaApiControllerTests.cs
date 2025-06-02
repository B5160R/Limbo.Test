using System.Net;
using Herningsholm.Web;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[Category("Integration")]
[Description("Integration tests for validating SPA API controller return data.")]
[TestOf(typeof(SpaApiControllerTests))]
public class SpaApiControllerTests : IntegrationTestBase<Program> {
    private readonly JsonHandler _jsonHandler = new JsonHandler();

    [SetUp]
    public override void Setup() {
        base.Setup();
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
    [TestCase(TestName = "SpaApiControllerTests - GetData returns expected data for block elements")]
    [Description("Test to ensure that the GetData method in the SpaApiController returns expected data for block elements.")]
    public async Task GetData_Returns_Expected_Data_With_BlockElement_ContentType() {
        // Arrange
        string pageName = "Alle block-elementer";
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=false&url=/da/limbotestarea/test-underforside/alle-block-elementer/&parts=content";

        var isNotSettingsData = new IsSettingsData(_invert: true);
        var isNotPage = new IsPage(_invert: true);
        var isNotVideoBlock = new IsVideoBlock(_invert: true);
        SpaContentCheck<TokensContainer> contentCheck = ContentCheckBuilder
            .CreateSpaCheck<SpaResponseBlockElementCheck, TokensContainer>(ServiceProvider)
            .WithRequirement(isNotSettingsData)
            .WithRequirement(isNotPage)
            .WithRequirement(isNotVideoBlock) // Video blocks are not accessible in the SPA response by key
            .Build();

        // Act
        // Get the content from the Umbraco backoffice
        var backofficeContentElements = GetBackofficeContentElements(pageName);
        // Get the content from the SPA API controller
        var spaGetDataResponse = await GetContentAsStringAsync(url);

        // Parse the JSON strings to JObjects
        var backofficeContentElementsParsedJson = _jsonHandler.ConvertJsonToJObject(backofficeContentElements);
        var spaResponseParsedJson = _jsonHandler.ConvertJsonToJObject(spaGetDataResponse);

        // This focuses on the elements on the document (page) only (ie. 'contentData' and 'contentElements')
        var backofficeTokens = backofficeContentElementsParsedJson.SelectTokens("$..contentData[?(@.udi)]");
        var spaResponseTokens = spaResponseParsedJson.SelectTokens("$..contentElements[?(@.key)]");

        await contentCheck.RunAssertionsAsync(_jsonHandler, backofficeTokens, spaResponseTokens);
    }
}