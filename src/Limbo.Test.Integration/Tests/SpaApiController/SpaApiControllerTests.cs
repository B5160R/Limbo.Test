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
                            spaSubLevelElements.Remove(spaSubLevelElement); // Remove the matched element to avoid duplicate checks
                            break; // Exit the loop if a match is found
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
}

public class JsonHandler {
    private readonly HashSet<string> _parsedStrings = new();

    public List<JToken> InspectForUdi(JToken token) {
        var result = new List<JToken>();

        // Check if the current token is an object and contains a "udi" property
        if (token.Type == JTokenType.Object) {
            var obj = (JObject) token;
            if (obj.TryGetValue("udi", out _)) {
                result.Add(obj); // Add the entire object containing the "udi" property
            }

            // Recursively inspect all properties of the object
            foreach (var prop in obj.Properties()) {
                result.AddRange(InspectForUdi(prop.Value));
            }
        }
        // Check if the current token is an array and inspect each element
        else if (token.Type == JTokenType.Array) {
            foreach (var item in (JArray) token) {
                result.AddRange(InspectForUdi(item));
            }
        }

        return result;
    }

    public List<JToken> InspectForKey(JToken token) {
        var result = new List<JToken>();

        // Check if the current token is an object and contains a "key" property
        if (token.Type == JTokenType.Object) {
            var obj = (JObject) token;
            if (obj.TryGetValue("key", out _)) {
                result.Add(obj); // Add the entire object containing the "key" property
            }

            // Recursively inspect all properties of the object
            foreach (var prop in obj.Properties()) {
                result.AddRange(InspectForKey(prop.Value));
            }
        }
        // Check if the current token is an array and inspect each element
        else if (token.Type == JTokenType.Array) {
            foreach (var item in (JArray) token) {
                result.AddRange(InspectForKey(item));
            }
        }

        return result;
    }

    public JObject ConvertJsonToJObject(string json) {
        JObject root = JObject.Parse(json);
        RecursivelyParseStrings(root);
        return root;
    }

    private JToken RecursivelyParseStrings(JToken token) {
        if (token.Type == JTokenType.Object) {
            // Traverse all properties in the object
            foreach (var prop in ((JObject) token).Properties()) {
                prop.Value = RecursivelyParseStrings(prop.Value);
            }
        } else if (token.Type == JTokenType.Array) {
            // Traverse all elements in the array
            var arr = (JArray) token;
            for (int i = 0; i < arr.Count; i++) {
                arr[i] = RecursivelyParseStrings(arr[i]);
            }
        } else if (token.Type == JTokenType.String) {
            // Parse strings that might contain JSON
            return ParseIfJsonString(token);
        }

        return token;
    }

    private JToken ParseIfJsonString(JToken token) {
        string str = token.ToString();

        // Skip parsing if the string has already been parsed
        if (_parsedStrings.Contains(str)) return token;

        // Check if the string contains JSON-like characters
        if (str.Contains("{") || str.Contains("[") || str.Contains("]") || str.Contains("}")) {
            try {
                // Attempt to parse the string as JSON
                var parsed = JToken.Parse(str);
                _parsedStrings.Add(str); // Cache the parsed string
                RecursivelyParseStrings(parsed); // Recursively parse the new JSON structure
                return parsed;
            } catch {
                // Not valid JSON, return the original string
            }
        }

        return token;
    }
}