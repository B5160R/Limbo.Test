using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

public class ContentTests : IntegrationTestBase {
    [TestCase(TestName = "All Umbraco content settings are configured correctly")]
    public async Task AllUmbracoContentSettingsAreConfiguredCorrectlyAsync() {

        // Arrange
        var documentTypeService = GetService<IContentTypeService>();
        var documentTypes = documentTypeService.GetAll();
        var propertyChecks = GetPropertyChecks();

        // Act
        Assert.Multiple(async() =>
        {
            foreach (var documentType in documentTypes) {
                // Combine properties from groups and no-group properties
                var allPropertyTypes = documentType.NoGroupPropertyTypes.Concat(
                    documentType.PropertyGroups.SelectMany(pg => pg.PropertyTypes
                        ?? Enumerable.Empty<IPropertyType>()));

                foreach (var property in allPropertyTypes) {

                    var propertyDetails = new PropertyDetails(
                        documentType.Name,
                        property,
                        documentType);

                    foreach (var check in propertyChecks) {
                        // Assert
                        await check.ValidateContentAsync(propertyDetails);
                    }
                }
            }
        });
    }

    private IReadOnlyCollection<ContentCheckBase<PropertyDetails>> GetPropertyChecks() {
        // Reusable requirements
        var doesNotUseMultipleTextstring = new UsesPropertyEditor("Umbraco.MultipleTextstring", invert: true);
        var doesNotUseRadioButtonList = new UsesPropertyEditor("Umbraco.RadioButtonList", invert: true);
        var doesNotUseUserPicker = new UsesPropertyEditor("Umbraco.UserPicker", invert: true);
        var doesNotUseLabel = new UsesPropertyEditor("Umbraco.Label", invert: true);
        var doesNotUseContactPersonPicker = new UsesPropertyEditor("ContactPersonPicker", invert: true);
        var doesNotUseTinyMce = new UsesPropertyEditor("Umbraco.TinyMCE", invert: true);
        var doesNotUseDateTime = new UsesPropertyEditor("Umbraco.DateTime", invert: true);
        var doesNotUseColorGuide = new UsesPropertyEditor("custom.ColorGuide", invert: true);
        var doesNotUseFormPicker = new UsesPropertyEditor("UmbracoForms.FormPicker", invert: true);
        var doesNotUseDataList = new UsesPropertyEditor("Umbraco.Community.Contentment.DataList", invert: true);

        var doesNotUseBlocklist = new UsesPropertyEditor("Umbraco.BlockList", invert: true);

        return new[]
        {
            ContentCheckBuilder.Create<PropertyHasNameAndAliasCheck, PropertyDetails>(ServiceProvider)
                .WithRequirement<RteHidesLabel>(true)
                .Go(),
            ContentCheckBuilder.Create<PropertyUsesLimboPropertyEditorCheck, PropertyDetails>(ServiceProvider)
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
                .Go()
            // ContentCheckBuilder.Create<PropertyOptionalPrefixCheck, PropertyDetails>(ServiceProvider)
            //     .WithRequirement<IsOptional>()
            //     .WithRequirement(doesNotUseEditorNotes)
            //     .WithRequirement(doesNotUseNotes)
            //     .WithRequirement<RteHidesLabel>(true)
            //     .Go()
        };
    }
}

// PropertyDetails class to hold the details of a property for validation
// Instead of just passing an IPropertyType to the checks, we pass a record with specific the details we need
// This allows us to have a better understanding of what we are validating and why
// and also allows us to pass in the content type for better error messages
public record PropertyDetails(string? DocumentTypeName, IPropertyType Property, IContentType ContentType);