using NUnit.Framework;

internal sealed class PropertyIsMandatoryCheck : ContentCheckBase<PropertyDetails> {
    public PropertyIsMandatoryCheck(IReadOnlyCollection<IContentRequirementBase<PropertyDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(PropertyDetails content) {
        Assert.That(content.Property.Mandatory, Is.True,
            $"Property '{content.Property.Name}' on document type '{content.DocumentTypeName}' should be mandatory.");
        return ValueTask.CompletedTask;
    }
}