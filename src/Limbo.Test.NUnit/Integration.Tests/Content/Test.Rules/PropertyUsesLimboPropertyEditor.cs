using NUnit.Framework;

internal sealed class PropertyUsesLimboPropertyEditorCheck : ContentCheckBase<PropertyDetails> {
    public PropertyUsesLimboPropertyEditorCheck(IReadOnlyCollection<IContentRequirementBase<PropertyDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(PropertyDetails content) {
        // asset that property editor alias begins with "Limbo" or "Skybrud"
        Assert.That(content.Property.PropertyEditorAlias, Does.StartWith("Limbo").Or.StartWith("Skybrud"),
            $"Property '{content.Property.Name}' on document type '{content.DocumentTypeName}' should use a Limbo property editor.");

        return ValueTask.CompletedTask;
    }
}