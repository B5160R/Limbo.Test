using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

public class ContentTests : IntegrationTestBase {
    [TestCase(TestName = "All Umbraco content settings are configured correctly")]
    public async Task AllUmbracoContentSettingsAreConfiguredCorrectlyAsync() {

        // Arrange
        var documentTypeService = GetService<IContentTypeService>();
        var localizationService = GetService<ILocalizationService>();

        var globalSettings = GetGlobalSettings();
        var defaultUILanguage = globalSettings.Value.DefaultUILanguage;

        var documentTypes = documentTypeService.GetAll();

        var propertyChecks = GetPropertyChecks();

        // Act
        foreach (var documentType in documentTypes) {
            // Combine properties from groups and no-group properties
            var allPropertyTypes = documentType.NoGroupPropertyTypes.Concat(
                documentType.PropertyGroups.SelectMany(pg => pg.PropertyTypes
                    ?? Enumerable.Empty<IPropertyType>()));

            foreach (var property in allPropertyTypes) {

                var propertyDetails = new PropertyDetails(
                    defaultUILanguage,
                    property.Name,
                    property.Description,
                    property,
                    documentType);

                // Validate all property versions
                foreach (var check in propertyChecks) {
                    // Assert
                    await check.ValidateContentAsync(propertyDetails);
                }
            }
        }
    }

    private IReadOnlyCollection<ContentCheckBase<PropertyDetails>> GetPropertyChecks() {
        // Reusable requirements
        var doesNotUseEditorNotes = new UsesPropertyEditor("Umbraco.Community.Contentment.EditorNotes", invert: true);
        var doesNotUseNotes = new UsesPropertyEditor("Umbraco.Community.Contentment.Notes", invert: true);

        return new[]
        {
            ContentCheckBuilder.Create<PropertyHasNameAndDescriptionCheck, PropertyDetails>(ServiceProvider)
                .WithRequirement(doesNotUseEditorNotes)
                .WithRequirement(doesNotUseNotes)
                .WithRequirement<RteHidesLabel>(true)
                .Go(),
            // ContentCheckBuilder.Create<PropertyOptionalPrefixCheck, PropertyDetails>(ServiceProvider)
            //     .WithRequirement<IsOptional>()
            //     .WithRequirement(doesNotUseEditorNotes)
            //     .WithRequirement(doesNotUseNotes)
            //     .WithRequirement<RteHidesLabel>(true)
            //     .Go()
        };
    }
}

public record PropertyDetails(string Culture, string? Name, string? Description, IPropertyType Property, IContentType ContentType);