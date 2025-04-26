using NUnit.Framework;
using Umbraco.Cms.Core.Models;

// Abstract base class for performing content checks with requirements
internal abstract class ContentCheckBase<T>(IReadOnlyCollection<IContentRequirementBase<T>> _requirements, string? projectInitials = null){

    // Validates the content by ensuring all requirements are met
    private ValueTask ValidateContentAsync(T content) {
        // Check each requirement
        foreach (var requirement in _requirements) {
            if (!requirement.IsRequirementMet(content)) {
                // If any requirement is not met, skip further validation
                return ValueTask.CompletedTask;
            }
        }
        // Perform the actual content validation
        return DoValidateContentAsync(content);
    }

    public async Task RunAssertionsAsync(IEnumerable<IContentType> contentTypes, bool runForProperties) {
        await Task.Run(() => {
            Assert.Multiple(async () => {
                foreach (var documentType in contentTypes) {

                    if (runForProperties is false) {
                        var propertyDetails = new DocumentTypeDetails(
                            documentType.Name,
                            null,
                            documentType,
                            documentType.IsElement ? true : false);
                        await ValidateContentAsync((T) (object) propertyDetails);
                        continue;
                    }

                    // Combine properties from groups and no-group properties
                    var allPropertyTypes = documentType.NoGroupPropertyTypes.Concat(
                        documentType.PropertyGroups.SelectMany(pg => pg.PropertyTypes
                            ?? Enumerable.Empty<IPropertyType>()));

                    foreach (var property in allPropertyTypes) {
                        // Create property details for validation
                        var propertyDetails = new DocumentTypeDetails(
                            documentType.Name,
                            property,
                            documentType,
                            documentType.IsElement ? true : false);

                        await ValidateContentAsync((T) (object) propertyDetails);
                    }
                }
            });
        });
    }

    // Abstract method to be implemented by derived classes for specific validation logic
    protected abstract ValueTask DoValidateContentAsync(T content);
}