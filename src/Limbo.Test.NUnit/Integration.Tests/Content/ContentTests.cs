using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

// Integration tests for validating Umbraco content settings
public class ContentTests : IntegrationTestBase {
    [TestCase(TestName = "Validate all Umbraco content settings")]
    public async Task ValidateAllUmbracoContentSettingsAsync() {

        // Arrange: Get all document types and property checks
        var documentTypeService = GetService<IContentTypeService>();
        var documentTypes = documentTypeService.GetAll();
        var propertyChecks = GetPropertyChecks();

        // Act: Validate each document type and its properties
        await Task.Run(() =>
        {
            Assert.Multiple(() =>
            {
                foreach (var documentType in documentTypes) {
                    // Combine properties from groups and no-group properties
                    var allPropertyTypes = documentType.NoGroupPropertyTypes.Concat(
                        documentType.PropertyGroups.SelectMany(pg => pg.PropertyTypes
                            ?? Enumerable.Empty<IPropertyType>()));

                    foreach (var property in allPropertyTypes) {
                        // Create property details for validation
                        var propertyDetails = new PropertyDetails(
                            documentType.Name,
                            property,
                            documentType);

                        // Validate the property against all checks
                        foreach (var check in propertyChecks) {
                            check.ValidateContentAsync(propertyDetails).GetAwaiter().GetResult();
                        }
                    }
                }
            });
        });
    }

    // Returns a collection of reusable property checks
    private IReadOnlyCollection<ContentCheckBase<PropertyDetails>> GetPropertyChecks() {
        // Define reusable requirements
        var doesNotUseMultipleTextstring = new UsesPropertyEditor("Umbraco.MultipleTextstring", _invert: true);
        var doesNotUseRadioButtonList = new UsesPropertyEditor("Umbraco.RadioButtonList", _invert: true);
        var doesNotUseUserPicker = new UsesPropertyEditor("Umbraco.UserPicker", _invert: true);
        var doesNotUseLabel = new UsesPropertyEditor("Umbraco.Label", _invert: true);
        var doesNotUseContactPersonPicker = new UsesPropertyEditor("ContactPersonPicker", _invert: true);
        var doesNotUseTinyMce = new UsesPropertyEditor("Umbraco.TinyMCE", _invert: true);
        var doesNotUseDateTime = new UsesPropertyEditor("Umbraco.DateTime", _invert: true);
        var doesNotUseColorGuide = new UsesPropertyEditor("custom.ColorGuide", _invert: true);
        var doesNotUseFormPicker = new UsesPropertyEditor("UmbracoForms.FormPicker", _invert: true);
        var doesNotUseDataList = new UsesPropertyEditor("Umbraco.Community.Contentment.DataList", _invert: true);
        var doesNotUseBlocklist = new UsesPropertyEditor("Umbraco.BlockList", _invert: true);

        // Return a collection of content checks
        return new[]
        {
            ContentCheckBuilder
                .Create<PropertyHasNameAndAliasCheck, PropertyDetails>(ServiceProvider)
                // .WithRequirement<RteHidesLabel>(true)
                .Build(),

            ContentCheckBuilder
                .Create<PropertyAliasIsUniqueCheck, PropertyDetails>(ServiceProvider)
                .Build(),

            ContentCheckBuilder
                .Create<PropertyUsesLimboPropertyEditorCheck, PropertyDetails>(ServiceProvider)
                .WithRequirement(doesNotUseMultipleTextstring)
                .WithRequirement(doesNotUseRadioButtonList)
                .WithRequirement(doesNotUseUserPicker)
                .WithRequirement(doesNotUseLabel)
                .WithRequirement(doesNotUseContactPersonPicker)
                .WithRequirement(doesNotUseTinyMce)
                .WithRequirement(doesNotUseDateTime)
                .WithRequirement(doesNotUseBlocklist)
                .WithRequirement(doesNotUseColorGuide)
                .WithRequirement(doesNotUseFormPicker)
                .WithRequirement(doesNotUseDataList)
                .Build()
        };
    }
}

// Record to hold property details for validation
public record PropertyDetails(string? DocumentTypeName, IPropertyType Property, IContentType ContentType);