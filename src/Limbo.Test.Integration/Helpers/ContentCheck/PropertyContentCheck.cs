using Umbraco.Cms.Core.Models;

// Abstract base class for performing content checks with requirements
internal abstract class PropertyContentCheck<T>(IReadOnlyCollection<IContentRequirementBase<T>> _requirements,
        string? projectInitials = null) {
    public async Task RunAssertionsAsync(IEnumerable<IContentType> contentTypes, bool runForSubProperties) {
        await Task.Run(() => {
            Assert.Multiple(async () => {
                foreach (var documentType in contentTypes) {

                    if (runForSubProperties is false) {
                        var propertyDetails = new DocumentTypeDetails(
                            documentType.Name,
                            null,
                            documentType,
                            documentType.IsElement ? true : false);
                        if (await ValidateRequirementsAsync((T) (object) propertyDetails)) {
                            await ValidateRulesContentAsync((T) (object) propertyDetails);
                        }
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

                        await ValidateRequirementsAsync((T) (object) propertyDetails);
                    }
                }
            });
        });
    }

    private ValueTask<bool> ValidateRequirementsAsync(T content) {
        return ValueTask.FromResult(_requirements.All(requirement => requirement.IsRequirementMet(content)));
    }

    protected abstract ValueTask ValidateRulesContentAsync(T content);
}